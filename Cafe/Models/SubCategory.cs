using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //////////////////////////////


        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int ImageId { get; set; }
        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }


        public virtual List<Product> Products { get; set; }


    }
}
