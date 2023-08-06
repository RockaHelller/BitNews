using System.ComponentModel.DataAnnotations;

namespace BitNews.ViewModels.Account
{
    public class LoginVM
    {
        public string EmailOrUsername { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
