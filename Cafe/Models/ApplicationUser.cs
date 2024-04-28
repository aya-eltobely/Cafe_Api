using Microsoft.AspNetCore.Identity;

namespace Cafe.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IsActivate { get; set; }

        /// ////////////////////////////////////////////////////



        //public virtual List<Address> Addresses  { get; set; }
        public virtual List<Order> Orders  { get; set; }

        public virtual Delivery Delivery  { get; set; }

    }
}
