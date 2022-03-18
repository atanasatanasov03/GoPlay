using GoPlayServer.Entities;

namespace GoPlayServer.DTOs
{
    public class ReportedPostDTO
    {
        public PostDTO reportedPost { get; set; }
        public DateTime timestamp { get; set; }
        public string reporter { get; set; }
        public string reason { get; set; }
        public bool? toBeRemoved { get; set; }
    }
}
