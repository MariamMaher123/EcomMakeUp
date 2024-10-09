using Microsoft.AspNetCore.Identity;

namespace EcomMakeUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BDay { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public string Photo { get; set; }
        public List<Payment> payments { get; set; }

        public List<OrderProduct> orders { get; set; }
    }
}
