using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PoeWebAPI.Models;

namespace PoeWebAPI.Models
{
    public class ServiceRequestAPI
    {
        [Key]
        public int ServiceRequestId { get; set; }
        public int ContractId { get; set; }
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }  // stored in ZAR

       public ContractStatusAPI Status { get; set; }

        public ContractAPI? Contract { get; set; }
    }
}