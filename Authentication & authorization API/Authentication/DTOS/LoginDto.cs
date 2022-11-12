using System.ComponentModel.DataAnnotations;

namespace Authentication.DTOS
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
