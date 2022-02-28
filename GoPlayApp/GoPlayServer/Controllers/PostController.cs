using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
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

        public PostController(AppDbContext context, IPostRepository postRepo, IUserRepository userRepo)
        {
            _context = context;
            _postRepo = postRepo;
            _userRepo = userRepo;
        }

        [HttpPost("createPlay")]
        public async Task<ActionResult<PlayPostDTO>> createPlayPost(NewPlayPostDTO newPlayPostDTO)
        {
            if (!await UserExists(newPlayPostDTO.userName)) return new BadRequestResult();

            var user = await _userRepo.GetUserByUsernameAsync(newPlayPostDTO.userName);

            var playPost = new PlayPost
            {
                userId = user.Id,
                heading = newPlayPostDTO.heading,
                content = newPlayPostDTO.content,
                address = newPlayPostDTO.address
            };

            _postRepo.AddPlayPost(playPost);
            await _context.SaveChangesAsync();

            return new PlayPostDTO
            {
                userName = user.userName,
                heading = playPost.heading,
                content = playPost.content,
                address = playPost.address
            };
        }

        [HttpPost("createNews")]
        public async Task<ActionResult<NewsPostDTO>> createNewsPost(NewNewsPostDTO newNewsPostDTO)
        {
            if (!await UserExists(newNewsPostDTO.userName)) return new BadRequestResult();

            var user = await _userRepo.GetUserByUsernameAsync(newNewsPostDTO.userName);

            var newsPost = new NewsPost
            {
                userId = user.Id,
                heading = newNewsPostDTO.heading,
                content = newNewsPostDTO.content,
                pictureUrl = newNewsPostDTO.pictureUrl
            };

            _postRepo.AddNewsPost(newsPost);
            await _context.SaveChangesAsync();

            return new NewsPostDTO
            {
                id = newsPost.Id,
                userName = user.userName,
                heading = newsPost.heading,
                content = newsPost.content,
                pictureUrl = newsPost.pictureUrl
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
                    userName = user.userName,
                    heading = playPost.heading,
                    content = playPost.content,
                    address = playPost.address
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
                    pictureUrl = newsPost.pictureUrl
                });
            }

            return Ok(newsPostsDTOs);
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.AppUsers.AnyAsync(x => x.userName == userName);
        }
    }
}
