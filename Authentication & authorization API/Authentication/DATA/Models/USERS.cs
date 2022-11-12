using Microsoft.AspNetCore.Identity;

namespace Authentication.DATA.Models
{
    public class USERS : IdentityUser
    {
        public string Role { get; set; } = "";
        public string SchoolName { get; set; } = "";
    }
}
