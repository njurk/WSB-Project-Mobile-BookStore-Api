namespace BookStoreApi.Models.DTOs
{
    public class CreateOrderItemDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
