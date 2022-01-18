namespace GoPlayServer.Entities
{
    public interface BaseUser : BaseEntity
    {
        public string userName { get; set; }
        public string address { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
