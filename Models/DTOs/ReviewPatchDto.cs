namespace BookStoreApi.Models.DTOs
{
    public class ReviewPatchDto
    {
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public int? UserId { get; set; }
    }
}
