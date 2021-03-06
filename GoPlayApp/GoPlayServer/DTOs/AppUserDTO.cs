namespace GoPlayServer.DTOs
{
    public class AppUserDTO
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? token { get; set; }
        public DateTime? mutedOn { get; set; }
        public int? mutedFor { get; set; }
        public bool banned { get; set; } = false;
        public bool verified { get; set; }
    }
}
