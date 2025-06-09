public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public string ImageUrl { get; set; } = null!;
}
