using Cafe.Models;
using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string deliveryStatus { get; set; }
        public int OrderTotals{ get; set; }
        public string UserFullName { get; set; }
        public string DeliveryFullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

    }
}
