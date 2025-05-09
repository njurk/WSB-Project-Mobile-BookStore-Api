namespace BookStoreApi.Models.DTOs
{
    public class LoginRequest
    {
        public string Identifier { get; set; }  // username/email
        public string Password { get; set; }
    }

}
