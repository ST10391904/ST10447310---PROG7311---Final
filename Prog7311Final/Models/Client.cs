namespace Prog7311Final.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactDetails { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        // Navigation property
        public List<Contract> Contracts { get; set; } = new();
    }
}