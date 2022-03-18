using GoPlayServer.Entities;

namespace GoPlayServer.DTOs
{
    public class ReportPostDTO
    {
        public Guid postId { get; set; }
        public string username { get; set; }
        public string reason { get; set; }
    }
}
