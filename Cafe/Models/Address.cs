using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Phone{ get; set; }


        ////////////////////////////////////////////
        //public string UserId { get; set; }
        //[ForeignKey("UserId")]
        //public virtual ApplicationUser AppUser { get; set; }


        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        //public virtual List<Order> Orders { get; set; }


    }
}
