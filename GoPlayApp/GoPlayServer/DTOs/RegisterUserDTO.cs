using System.ComponentModel.DataAnnotations;

namespace GoPlayServer.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string address { get; set; }
    }
}
