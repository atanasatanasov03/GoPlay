using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Hubs;
using GoPlayServer.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoPlayServer.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;
        private readonly IGroupRepository _groupRepo;

        public PostController(AppDbContext context, IPostRepository postRepo, IUserRepository userRepo,
            IGroupRepository groupRepo)
        {
            _context = context;
            _postRepo = postRepo;
            _userRepo = userRepo;
            _groupRepo = groupRepo;
        }

        [HttpPost("createPost")]
        public async Task<ActionResult<PostDTO>> createPlayPost(NewPostDTO newPostDTO)
        {
            if (!await UserExists(newPostDTO.userName)) return new BadRequestResult();

            var user = await _userRepo.GetUserByUsernameAsync(newPostDTO.userName);

            if (user == null) return new BadRequestResult();
            if (user.groups == null) user.groups = new List<Group>();

            Post post;
            string groupname = null;

            if (newPostDTO.play)
            {
                var groupToAdd = new Group{
                    groupName = newPostDTO.userName + "'s group for " + newPostDTO.heading,
                    users = new List<AppUser>()
                };
                groupToAdd.users.Add(user);
                user.groups.Add(groupToAdd);
                
                await _groupRepo.AddGroupAsync(groupToAdd);
                _userRepo.Update(user);
                await _context.SaveChangesAsync();

                groupname = groupToAdd.groupName;
                post = new Post
                {
                    userId = user.Id,
                    heading = newPostDTO.heading,
                    content = newPostDTO.content,
                    timeOfCreation = DateTime.Now,
                    play = newPostDTO.play,

                    address = newPostDTO.address
                };
            } else
            {
                post = new Post
                {
                    userId = user.Id,
                    heading = newPostDTO.heading,
                    content = newPostDTO.content,
                    timeOfCreation = DateTime.Now,
                    play = newPostDTO.play,

                    pictureUrl = newPostDTO.pictureUrl
                };
            }

            if (newPostDTO.play)
            {
                Group group = await _groupRepo.GetGroupByNameAsync(groupname);
                post.groupId = group.Id;
            }
            await _postRepo.AddPostAsync(post);
            await _context.SaveChangesAsync();

            return new PostDTO
            {
                userName = user.userName,
                heading = post.heading,
                content = post.content,
                timeOfCreation = post.timeOfCreation,
                play = post.play,

                address = post.address,
                groupName = groupname,
                pictureUrl = post.pictureUrl
            };
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<PostDTO>>> getAllPosts()
        {
            var playPosts = await _postRepo.GetPlayPostsAsync();
            var newsPosts = await _postRepo.GetNewsPostsAsync();
            var postDTOs = new List<PostDTO>();

            AppUser user;

            for(int i = 0, j = 0; i < playPosts.Count(); i++)
            {
                var group = await _groupRepo.GetGroupByIdAsync(playPosts.ElementAt(i).groupId);
                user = await _userRepo.GetUserByIdAsync(playPosts.ElementAt(i).userId);

                postDTOs.Add(new PostDTO
                {
                    postId = playPosts.ElementAt(i).Id,
                    userName = user.userName,
                    heading = playPosts.ElementAt(i).heading,
                    content = playPosts.ElementAt(i).content,
                    timeOfCreation = playPosts.ElementAt(i).timeOfCreation,
                    play = playPosts.ElementAt(i).play,

                    address = playPosts.ElementAt(i).address,
                    groupName = group.groupName
                });

                if ((i + 1) % 5 == 0 || (i+1) == playPosts.Count())
                {
                    user = await _userRepo.GetUserByIdAsync(newsPosts.ElementAt(j).userId);
                    postDTOs.Add(new PostDTO
                    {
                        postId = newsPosts.ElementAt(j).Id,
                        userName = user.userName,
                        heading = newsPosts.ElementAt(j).heading,
                        content = newsPosts.ElementAt(j).content,
                        timeOfCreation = newsPosts.ElementAt(j).timeOfCreation,
                        play = newsPosts.ElementAt(j).play,

                        pictureUrl = newsPosts.ElementAt(j).pictureUrl
                    });
                    j++;
                }
            }

            return Ok(postDTOs);
        }

        [HttpGet("getPostsFor")]
        public async Task<ActionResult<List<Post>>> getPostsFor(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            if (user == null) return new BadRequestResult();

            var posts = await _postRepo.GetPostsByUserIdAsync(user.Id);
            if (posts == null) return NotFound();
            posts.OrderBy(post => post.timeOfCreation);

            var postDTOs = new List<PostDTO>();

            foreach (var post in posts)
            {
                postDTOs.Add(await BuildPostDTO(post));
            }

            return Ok(postDTOs);
        }

        [HttpPost("report")]
        public async Task<ActionResult> ReportPost(ReportPostDTO reportDTO)
        {
            var user = await _userRepo.GetUserByUsernameAsync(reportDTO.username);
            if (user == null) return new BadRequestResult();
            
            var report = new ReportedPost
            {
                reportedPostId = reportDTO.postId,
                timestamp = DateTime.Now,
                reporterId = user.Id,
                reason = reportDTO.reason
            };

            await _postRepo.ReportPostAsync(report);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("reported")]
        public async Task<ActionResult<List<ReportedPostDTO>>> GetReportedPosts()
        {
            var reports = await _postRepo.GetReportedPostsAsync();
            var reportedPostDTOs = new List<ReportedPostDTO>();

            foreach(var report in reports)
            {
                var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == report.reportedPostId);
                var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.Id == report.reporterId);
                reportedPostDTOs.Add(new ReportedPostDTO
                {
                    reportedPost = await BuildPostDTO(post),
                    timestamp = report.timestamp,
                    reporter = user.userName,
                    reason = report.reason
                });
            }

            return Ok(reportedPostDTOs);
        }

        [HttpPost("resolveReport")]
        public async Task<ActionResult> ResolveReport(ReportedPostDTO reportDTO)
        {
            var reportedPost = await _context.ReportedPosts.SingleOrDefaultAsync(rp => rp.reportedPostId == reportDTO.reportedPost.postId);
            if (reportedPost is null) return new BadRequestResult();

            _postRepo.ResolveReport(reportedPost);
            if(reportDTO.toBeRemoved == true)
            {
                var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == reportedPost.reportedPostId);
                _postRepo.RemovePost(post);
            } 
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<PostDTO> BuildPostDTO(Post post)
        {
            PostDTO postDTO;

            var user = await _userRepo.GetUserByIdAsync(post.userId);
            if (post.play)
            {
                var group = await _groupRepo.GetGroupByIdAsync(post.groupId);
                postDTO = new PostDTO
                {
                    postId = post.Id,
                    userName = user.userName,
                    heading = post.heading,
                    content = post.content,
                    timeOfCreation = post.timeOfCreation,
                    play = post.play,

                    address = post.address,
                    groupName = group.groupName
                };
            }
            else
            {
                postDTO = new PostDTO
                {
                    postId = post.Id,
                    userName = user.userName,
                    heading = post.heading,
                    content = post.content,
                    timeOfCreation = post.timeOfCreation,
                    play = post.play,

                    pictureUrl = post.pictureUrl
                };
            }

            return postDTO;
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.AppUsers.AnyAsync(x => x.userName == userName);
        }
    }
}
