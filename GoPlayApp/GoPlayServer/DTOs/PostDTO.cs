namespace GoPlayServer.DTOs
{
    public class PostDTO
    {
        public Guid postId { get; set; }
        public string userName { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public DateTime timeOfCreation { get; set; }
        public bool play { get; set; }

        public DateTime? expires { get; set; }
        public string? address { get; set; }
        public string? groupName { get; set; }
        public string? pictureUrl { get; set; }
    }
}
