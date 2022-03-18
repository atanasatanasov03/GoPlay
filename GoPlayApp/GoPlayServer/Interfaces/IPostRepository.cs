using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPlayPostsAsync();
        Task<IEnumerable<Post>> GetNewsPostsAsync();
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid id);
        Task AddPostAsync(Post post);
        Task ReportPostAsync(ReportedPost reported);
        Task<IEnumerable<ReportedPost>> GetReportedPostsAsync();
        void ResolveReport(ReportedPost reported);
        void RemovePost(Post post);
    }
}
