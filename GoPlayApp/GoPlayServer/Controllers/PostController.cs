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

        [HttpPost("createPlay")]
        public async Task<ActionResult<PlayPostDTO>> createPlayPost(NewPlayPostDTO newPlayPostDTO)
        {
            if (!await UserExists(newPlayPostDTO.userName)) return new BadRequestResult();

            var user = await _userRepo.GetUserByUsernameAsync(newPlayPostDTO.userName);

            if (user == null) return new BadRequestResult();

            var group = new Group
            {
                groupName = string.Concat(newPlayPostDTO.content.Substring(0, 5), newPlayPostDTO.heading.Substring(0, 5)),
                users = new List<AppUser>()
            };
            group.users.Add(user);
            if (user.groups == null) user.groups = new List<Group>();
            user.groups.Add(group);
            
            var playPost = new PlayPost
            {
                userId = user.Id,
                heading = newPlayPostDTO.heading,
                content = newPlayPostDTO.content,
                address = newPlayPostDTO.address,
                groupName = group.groupName,
                timeOfCreation = DateTime.Now
            };

            _postRepo.AddPlayPost(playPost);
            _groupRepo.AddGroup(group);
            _userRepo.Update(user);
            
            await _context.SaveChangesAsync();

            return new PlayPostDTO
            {
                userName = user.userName,
                heading = playPost.heading,
                content = playPost.content,
                address = playPost.address,
                groupName = playPost.groupName,
                timeOfCreation = playPost.timeOfCreation
            };
        }

        [HttpPost("createNews")]
        public async Task<ActionResult<NewsPostDTO>> createNewsPost(NewNewsPostDTO newNewsPostDTO)
        {
            if (!await UserExists(newNewsPostDTO.userName)) return new BadRequestResult();

            var user = await _userRepo.GetUserByUsernameAsync(newNewsPostDTO.userName);
            if (user.role != "center") return new ForbidResult();

            var newsPost = new NewsPost
            {
                userId = user.Id,
                heading = newNewsPostDTO.heading,
                content = newNewsPostDTO.content,
                pictureUrl = newNewsPostDTO.pictureUrl,
                timeOfCreation = DateTime.Now
            };

            _postRepo.AddNewsPost(newsPost);
            await _context.SaveChangesAsync();

            return new NewsPostDTO
            {
                id = newsPost.Id,
                userName = user.userName,
                heading = newsPost.heading,
                content = newsPost.content,
                pictureUrl = newsPost.pictureUrl,
                timeOfCreation = newsPost.timeOfCreation
            };
        }

        [HttpGet("getAllPlay")]
        public async Task<ActionResult<List<PlayPostDTO>>> getAllPlayPosts()
        {
            var playPosts = await _postRepo.GetPlayPostsAsync();
            var playPostsDTOs = new List<PlayPostDTO>();

            foreach(var playPost in playPosts)
            {
                var user = await _userRepo.GetUserByIdAsync(playPost.userId);
                playPostsDTOs.Add(new PlayPostDTO{
                    Id = playPost.Id,
                    userName = user.userName,
                    heading = playPost.heading,
                    content = playPost.content,
                    address = playPost.address,
                    groupName = playPost.groupName,
                    timeOfCreation = playPost.timeOfCreation
                });
            }

            return Ok(playPostsDTOs);
        }

        [HttpGet("getAllNews")]
        public async Task<ActionResult<List<NewsPostDTO>>> getAllNewsPosts()
        {
            var newsPosts = await _postRepo.GetNewsPostsAsync();
            var newsPostsDTOs = new List<NewsPostDTO>();

            foreach (var newsPost in newsPosts)
            {
                var user = await _userRepo.GetUserByIdAsync(newsPost.userId);
                newsPostsDTOs.Add(new NewsPostDTO
                {
                    id = newsPost.Id,
                    userName = user.userName,
                    heading = newsPost.heading,
                    content = newsPost.content,
                    pictureUrl = newsPost.pictureUrl,
                    timeOfCreation = newsPost.timeOfCreation
                });
            }

            return Ok(newsPostsDTOs);
        }

        [HttpGet("getPlayPostsFor")]
        public async Task<ActionResult<List<PlayPost>>> getPlayPostsFor(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            if (user == null) return new BadRequestResult();

            var playPosts = await _postRepo.GetPlayPostsByUserIdAsync(user.Id);
            if (playPosts == null) return NotFound();

            var playPostsDTOs = new List<PlayPostDTO>();

            foreach (var playPost in playPosts)
            {
                playPostsDTOs.Add(new PlayPostDTO
                {
                    userName = username,
                    heading = playPost.heading,
                    content = playPost.content,
                    address = playPost.address,
                    timeOfCreation = playPost.timeOfCreation
                });
            }

            return Ok(playPostsDTOs);
        }

        [HttpGet("getNewsPostsFor")]
        public async Task<ActionResult<List<PlayPost>>> getNewsPostsFor(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            if (user == null || user.role != "center") return new BadRequestResult();

            var newsPosts = await _postRepo.GetNewsPostsByUserIdAsync(user.Id);
            if (newsPosts == null) return NotFound();

            var newsPostsDTOs = new List<NewsPostDTO>();
            foreach (var newsPost in newsPosts)
            {
                newsPostsDTOs.Add(new NewsPostDTO
                {
                    userName = username,
                    heading = newsPost.heading,
                    content = newsPost.content,
                    pictureUrl = newsPost.pictureUrl,
                    timeOfCreation = newsPost.timeOfCreation
                });
            }

            return Ok(newsPostsDTOs);
        }

        [HttpPost("report")]
        public async Task<ActionResult> ReportPost(ReportPostDTO reportdto)
        {
            var user = await _userRepo.GetUserByUsernameAsync(reportdto.username);
            if (user == null) return new BadRequestResult();

            var report = new ReportedPost
            {
                reportedPostId = reportdto.postId,
                timestamp = DateTime.Now,
                reporterId = user.Id,
                reason = reportdto.reason
            };

            _postRepo.ReportPost(report);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("reported")]
        public async Task<ActionResult<List<ReportedPost>>> GetReportedPosts()
        {
            var reportedPosts = await _postRepo.GetReportedPosts();
            var reportedPostsDTOs = new List<ReportedPostDTO>();

            foreach(var post in reportedPosts)
            {
                var playPost = await _context.PlayPosts.SingleOrDefaultAsync(p => p.Id == post.reportedPostId);
                var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.Id == post.reporterId);
                reportedPostsDTOs.Add(new ReportedPostDTO
                {
                    reportedPost = playPost,
                    timestamp = post.timestamp,
                    reporter = user,
                    reason = post.reason
                });
            }

            return Ok(reportedPostsDTOs);
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.AppUsers.AnyAsync(x => x.userName == userName);
        }
    }
}
