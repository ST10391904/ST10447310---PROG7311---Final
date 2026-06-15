using PoeWebAPI.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoeWebAPI.Models
{
    public class ContractAPI
    {
        [Key]
         public int ContractId { get; set; }
        public string? ContractName { get; set; }
       
        public int ClientId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ContractStatusAPI Status { get; set; }
        public string ServiceLevel { get; set; } = string.Empty;
        public string? SignedAgreement { get; set; } 

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public decimal AmountInZAR { get; set; }
       
         public string? FileName { get; set; }

    public string? FilePath { get; set; }

    [NotMapped]
    public IFormFile? UploadFile { get; set; }

        // Navigation properties
        public ClientAPI? Client { get; set; }
        public ICollection<ServiceRequestAPI> ServiceRequests { get; set; } = new List<ServiceRequestAPI>();
    }
}