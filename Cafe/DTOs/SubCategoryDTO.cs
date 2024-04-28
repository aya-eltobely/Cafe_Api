using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class SubCategoryDTO :ImageDTO
    {
        [Required]
    
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

    }
}
