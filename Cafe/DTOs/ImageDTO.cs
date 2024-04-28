using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class ImageDTO
    {
        [Required]
        public string FileName { get; set; }
        [Required]

        public string ImageData { get; set; }
        [Required]

        public string ContentType { get; set; }
    }
}
