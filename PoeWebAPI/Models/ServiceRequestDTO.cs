namespace PoeWebAPI.Models
{
    public class ServiceRequestDTO
    {
        public int ContractId { get; set; }
        public string ContractName { get; set; } = string.Empty;
        public string? Name { get; set; }

        public ContractStatusAPI Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public decimal AmountInZAR { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}