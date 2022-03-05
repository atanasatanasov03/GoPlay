namespace GoPlayServer.Entities
{
    public class NewsPost : BasePost
    {
        public Guid Id { get; set; }
        public Guid userId { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public string pictureUrl { get; set; }
        public DateTime timeOfCreation { get; set; }
    }
}
