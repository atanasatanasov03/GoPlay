namespace GoPlayServer.Entities
{
    public interface BasePost : BaseEntity
    {
        public  Guid Id { get; set; }
        public Guid userId { get; set; }
        public string heading { get; set; }
        public string content { get; set; }
    }
}
