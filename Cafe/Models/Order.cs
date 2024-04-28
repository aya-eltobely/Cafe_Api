using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class Order
    {
        public int Id { get; set; }


        public DateTime Date { get; set; }
        public ProductStatusEnum Status { get; set; }
        public DeliveryStatusEnum DeliveryStatus { get; set; }
        public int Total { get; set; }

        /////////////////////////////

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser AppUser { get; set; }

        //public int AddressId { get; set; }
        //[ForeignKey("AddressId")]
        public virtual Address Address { get; set; }


        public virtual List<OrderItem> OrderItems { get; set; }


        public int? DeliveryId { get; set; }
        [ForeignKey("DeliveryId")]
        public virtual Delivery Delivery { get; set; }


    }
}
