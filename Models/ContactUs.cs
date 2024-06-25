namespace Restaurant_WebApp.Models
{
    public class ContactUs
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsNew { get; set; }
        public string? Subject { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
