using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class Delivery 
    {
        public int Id { get; set; }
       

        /// ///////////////////////////////


        public virtual List<Order> Orders { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual ApplicationUser AppUser { get; set; }

    }
}
