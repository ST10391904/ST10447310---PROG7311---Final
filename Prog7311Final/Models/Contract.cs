using Prog7311Final.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prog7311Final.Models
{
    public class Contract
    {
        public string? ContractName { get; set; }
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ContractStatus Status { get; set; }
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
        public Client? Client { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}