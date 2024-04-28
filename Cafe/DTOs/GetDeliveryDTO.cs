namespace Cafe.DTOs
{
    public class GetDeliveryDTO: GetUserDTO
    {
        public int DeliveryId { get; set; }
        public string PhoneNumber { get; set; }

    }
}
