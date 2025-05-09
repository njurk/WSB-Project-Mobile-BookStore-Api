namespace BookStoreApi.Models.DTOs
{
    public class ReviewCreateDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
