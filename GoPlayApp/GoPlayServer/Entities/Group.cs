namespace GoPlayServer.Entities
{
    public class Group : BaseEntity
    {
        public Group()
        {
            this.users = new List<AppUser>();
            this.messages = new List<Message>();
        }

        public Guid Id { get; set; }
        public string groupName { get; set; }
        public virtual List<AppUser> users { get; set; }
        public virtual List<Message> messages { get; set; }
    }
}
