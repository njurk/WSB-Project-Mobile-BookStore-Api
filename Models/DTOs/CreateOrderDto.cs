namespace BookStoreApi.Models.DTOs
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int DeliveryTypeId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }
}
