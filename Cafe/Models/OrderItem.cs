using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }


        ///////////
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }


    }
}
