using Microsoft.AspNetCore.Identity;

namespace BitNews.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
