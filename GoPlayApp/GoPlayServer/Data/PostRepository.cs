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

        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        } 

        public async Task<IEnumerable<Post>> GetPlayPostsAsync()
        {
            return await _context.Posts.Where(p => p.play == true).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetNewsPostsAsync()
        {
            return await _context.Posts.Where(p => p.play == false).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid id)
        {
            return await _context.Posts.Where(p => p.userId == id).ToListAsync();
        }

        public async Task ReportPostAsync(ReportedPost reported)
        {
            await _context.ReportedPosts.AddAsync(reported);
        }

        public async Task<IEnumerable<ReportedPost>> GetReportedPostsAsync()
        {
            return await _context.ReportedPosts.ToListAsync();
        }

        public void ResolveReport(ReportedPost reportedPost)
        {
            _context.ReportedPosts.Remove(reportedPost);
        }

        public void RemovePost(Post post)
        {
            _context.Posts.Remove(post);
        }
    }
}
