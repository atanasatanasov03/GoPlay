namespace GoPlayServer.Entities
{
    public class PlayPost : BasePost
    {
        public Guid Id { get; set; }
        public Guid userId { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public string address { get; set; }
    }
}
