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
    }
}
