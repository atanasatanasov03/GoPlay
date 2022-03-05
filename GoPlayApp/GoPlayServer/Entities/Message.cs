namespace GoPlayServer.Entities
{
    public class Message : BaseEntity
    {
        public Guid Id { get; set; } 
        public string text { get; set; }
        public string userName { get; set; }
        public DateTime date { get; set; }
        public Guid GroupId { get; set; }
        public virtual Group group { get; set; }
    }
}
