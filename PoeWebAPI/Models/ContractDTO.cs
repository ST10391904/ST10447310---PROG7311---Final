namespace PoeWebAPI.Models

{
public class ContractDTO
    {
    public string? ContractName { get; set; }
    public int ClientID { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ContractStatusAPI Status { get; set; }

    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    }
}