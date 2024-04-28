using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class CategoryDTO
    {
        [Required]
        //[RegularExpression("^[A-Za-z]+$")]
        public string Name { get; set; }

    }
}
