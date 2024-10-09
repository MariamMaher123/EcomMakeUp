using System.ComponentModel.DataAnnotations.Schema;

namespace EcomMakeUp.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        [ForeignKey("Productcs")]
        public int ProductId { get; set; }
        public int countProduct { get; set; }
        public string status { get; set; } = "Waiting";
        public DateTime DateAdd { get; set; }= DateTime.Now;    

        [ForeignKey("User")]
        public string  UserId { get; set; }


        public decimal ProductPrice { get; set; }
        public Products Productcs { get; set; }
       public ApplicationUser User { get; set; }
    }
}
