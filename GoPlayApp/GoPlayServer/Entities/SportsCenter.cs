namespace GoPlayServer.Entities
{
    public class SportsCenter : BaseUser
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string address { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        //public List<string> sportsAvailable { get; set; }
    }
}
