namespace EcomMakeUp.Dtos
{
    public class UpdateProfileDTO
    {

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BDay { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public IFormFile Photo { get; set; }
        public string Phone { get; set; }
    }
}
