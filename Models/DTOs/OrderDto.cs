namespace BookStoreApi.Models.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatusName { get; set; } = null!;
        public string DeliveryTypeName { get; set; } = null!;
        public decimal DeliveryFee { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public int TotalItemCount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
