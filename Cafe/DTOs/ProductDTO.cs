
using Cafe.Models;
using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class ProductDTO :  ImageDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }


        [Required]
        public string Description { get; set; }

    

        [Required]
        public int subCategoryId { get; set; }

    }
}
