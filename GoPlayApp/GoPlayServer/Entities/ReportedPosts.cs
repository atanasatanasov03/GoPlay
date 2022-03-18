namespace GoPlayServer.Entities
{
    public class ReportedPost : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid reportedPostId { get; set; }
        public DateTime timestamp { get; set; }
        public Guid reporterId { get; set; }
        public string reason { get; set; }
    }
}
