namespace GoPlayServer.DTOs
{
    public class NewPostDTO
    {
        public string userName { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public bool play { get; set; }
        
        public string? address { get; set; }
        public string? pictureUrl { get; set; }
    }
}
