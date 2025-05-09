namespace BookStoreApi.Models.DTOs
{
    public class CartCreateDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }

}
