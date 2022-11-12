using System.ComponentModel.DataAnnotations;

namespace Authentication.DTOS
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";

        public string Role  { get; set; } = "";

        public string SchoolName { get; set; } = "";
    }
}

