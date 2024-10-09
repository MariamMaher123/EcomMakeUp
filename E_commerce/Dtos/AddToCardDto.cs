using EcomMakeUp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomMakeUp.Dtos
{
    public class AddToCardDto
    {

        public int ProductId { get; set; }
        public int countProduct { get; set; }



       
    }
}
