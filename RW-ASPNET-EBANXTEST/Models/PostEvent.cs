namespace RW_ASPNET_EBANXTEST.Models
{

    public class PostEvent
    {
        public string type { get; set; }
        public decimal amount { get; set; }
        public string? origin { get; set; }
        public string? destination { get; set; }

    }
}
