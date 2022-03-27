namespace GoPlayServer.Entities
{
    public class Post : BaseEntity
    {
        public  Guid Id { get; set; }
        public Guid userId { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
        public DateTime timeOfCreation { get; set; }
        public bool play { get; set; }
        public string? address { get; set; }
        public Guid? groupId { get; set; }
        public DateTime? timeOfMeeting { get; set; }
        public virtual Group group { get; set; }
        public string? pictureUrl { get; set; }
    }
}
