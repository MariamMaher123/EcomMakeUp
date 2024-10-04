using System.ComponentModel.DataAnnotations.Schema;

namespace EcomMakeUp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;


        public string Status { get; set; } = "waiting";

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
