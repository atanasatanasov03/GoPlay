namespace GoPlayServer.Entities
{
    public class AppUser : BaseEntity
    {
        public Guid Id { get; set; }
        public string userName { get; set; }
        public string role { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? age { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string sports { get; set; }
        public virtual ICollection<Group> groups { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
