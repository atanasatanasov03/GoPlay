namespace GoPlayServer.Entities
{
    public class Group : BaseEntity
    {
        public Guid Id { get; set; }
        public string groupName { get; set; }
        public virtual ICollection<AppUser> users { get; set; }
        public virtual ICollection<Message> messages { get; set; }
    }
}
