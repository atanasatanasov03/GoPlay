using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<PlayPost>> GetPlayPostsAsync();
        Task<IEnumerable<NewsPost>> GetNewsPostsAsync();
        Task<IEnumerable<ReportedPost>> GetReportedPosts();
        Task<IEnumerable<NewsPost>> GetNewsPostsByUserIdAsync(Guid id);
        Task<IEnumerable<PlayPost>> GetPlayPostsByUserIdAsync(Guid id);
        void AddPlayPost(PlayPost playPost);
        void AddNewsPost(NewsPost newsPost);
        void ReportPost(ReportedPost reported);
    }
}
