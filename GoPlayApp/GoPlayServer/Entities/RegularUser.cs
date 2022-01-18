namespace GoPlayServer.Entities
{
    public class RegularUser : BaseUser
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string address { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int Age { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        //public List<string> favouriteSports { get; set; }
    }
}
