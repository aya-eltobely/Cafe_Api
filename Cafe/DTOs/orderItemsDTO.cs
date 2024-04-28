using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class orderItemsDTO
    {
        [Required]
        public int productId { get; set; }

        [Required]
        public int subTotal { get; set; }

        [Required]
        public int quantity { get; set; }
    }
}
