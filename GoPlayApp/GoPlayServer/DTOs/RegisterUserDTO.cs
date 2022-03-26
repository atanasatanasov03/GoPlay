using System.ComponentModel.DataAnnotations;

namespace GoPlayServer.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string role { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string address { get; set; }

        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? age { get; set; }
        public string sports { get; set; }
    }
}
