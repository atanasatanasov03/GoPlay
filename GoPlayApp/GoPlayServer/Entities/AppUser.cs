namespace GoPlayServer.Entities
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            this.groups = new List<Group>();
        }

        public Guid Id { get; set; }
        public string userName { get; set; }
        public string role { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? age { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string sports { get; set; }
        public virtual List<Group> groups { get; set; }
        public int? mutedFor { get; set; }
        public DateTime? mutedOn { get; set; }
        public bool banned { get; set; } = false;
        public bool validEmail { get; set; } = false;
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
