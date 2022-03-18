using GoPlayServer.Entities;

namespace GoPlayServer.DTOs
{
    public class ReportedPostDTO
    {
        public PlayPost reportedPost { get; set; }
        public DateTime timestamp { get; set; }
        public AppUser reporter { get; set; }
        public string reason { get; set; }
    }
}
