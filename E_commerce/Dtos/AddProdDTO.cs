namespace EcomMakeUp.Dtos
{
    public class AddProdDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public int Available_Quan { get; set; }
        public string Brand { get; set; }
    }
}
