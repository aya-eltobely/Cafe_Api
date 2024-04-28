using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class ProductPrices
    {
        public int Id { get; set; }
        public ProductSizeEnum Size { get; set; }
        public int price { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
