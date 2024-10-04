using System.ComponentModel.DataAnnotations.Schema;

namespace EcomMakeUp.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        [ForeignKey("Productcs")]
        public int ProductId { get; set; }
        public int countProduct { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public decimal ProductPrice { get; set; }
        public Products Productcs { get; set; }
        public Order Order { get; set; }
    }
}
