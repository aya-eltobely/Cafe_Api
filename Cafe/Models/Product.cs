using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public string Description { get; set; }

        /////////////////////////////////////////////// 

        public virtual List<OrderItem> OrderItems  { get; set; }

        public virtual List<ProductPrices> ProductPrices { get; set; }

        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        public int ImageId { get; set; }
        [ForeignKey("ImageId")]
        public virtual Image Image   { get; set; }

    }
}
