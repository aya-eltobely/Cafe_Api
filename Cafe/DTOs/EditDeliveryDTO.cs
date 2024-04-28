using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class EditDeliveryDTO
    {
        [DataType(DataType.EmailAddress)]
        public string  Email{ get; set; }

  
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
