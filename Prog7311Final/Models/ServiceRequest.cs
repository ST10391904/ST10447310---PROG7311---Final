using System.ComponentModel.DataAnnotations.Schema;
using Prog7311Final.Models;

namespace Prog7311Final.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public int ContractId { get; set; }
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }  // stored in ZAR

        public string Status { get; set; } = string.Empty;

        public Contract? Contract { get; set; }
    }
}