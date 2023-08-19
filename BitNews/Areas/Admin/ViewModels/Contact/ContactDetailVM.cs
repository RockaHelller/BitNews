using BitNews.Models;

namespace BitNews.Areas.Admin.ViewModels.Contact
{
    public class ContactDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatorName { get; set; }
        public MailModel EmailModel { get; set; }


    }
}
