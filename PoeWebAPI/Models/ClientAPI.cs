using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoeWebAPI.Models
{
    public class ClientAPI
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactDetails { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        // Navigation property
        public List<ContractAPI> Contracts { get; set; } = new();
    }
}