namespace EcomMakeUp.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int Available_Quan { get; set; }
        public string Brand { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
