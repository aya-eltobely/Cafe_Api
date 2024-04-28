using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class orderDTO 
    {
        [Required]
        public string name { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }


        [Required]
        public string state { get; set; }

        [Required]
        public string city { get; set; }


        [Required]
        public string street { get; set; }

        [Required]
        public string phone { get; set; }


        [Required]
        public int total { get; set; }

        public List<orderItemsDTO> items { get; set; }
    }
}
