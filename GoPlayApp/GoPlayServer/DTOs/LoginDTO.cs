using System.ComponentModel.DataAnnotations;

namespace GoPlayServer.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}