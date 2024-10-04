using System.ComponentModel.DataAnnotations.Schema;

namespace EcomMakeUp.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime PayDate { get; set; }
        public string Status { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
