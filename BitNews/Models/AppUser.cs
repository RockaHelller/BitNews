using Microsoft.AspNetCore.Identity;

namespace BitNews.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string UserAddress { get; set; }
        public string Image { get; set; }

        //public Basket Basket { get; set; }
        //public WishList WishList { get; set; }
        //public ICollection<Review> Reviews { get; set; }
    }
}
