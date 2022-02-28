namespace GoPlayServer.DTOs
{
    public class NewsPostDTO
    {
        public Guid id { get; set; }
        public string userName { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public string pictureUrl { get; set; }
    }
}
