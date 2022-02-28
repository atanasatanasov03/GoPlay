using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoPlayServer.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddNewsPost(NewsPost newsPost) => _context.NewsPosts.Add(newsPost);

        public void AddPlayPost(PlayPost playPost) => _context.PlayPosts.Add(playPost);

        public async Task<IEnumerable<NewsPost>> GetNewsPostsAsync()
        {
            return await _context.NewsPosts.ToListAsync();
        }

        public async Task<IEnumerable<NewsPost>> GetNewsPostsByUserIdAsync(Guid id)
        {
            return await _context.NewsPosts.Where(p => p.userId == id).ToListAsync();
        }

        public async Task<IEnumerable<PlayPost>> GetPlayPostsAsync()
        {
            return await _context.PlayPosts.ToListAsync();
        }

        public async Task<IEnumerable<PlayPost>> GetPlayPostsByUserIdAsync(Guid id)
        {
            return await _context.PlayPosts.Where(p => p.userId == id).ToListAsync();
        }
    }
}
