namespace Prog7311Final.Models

{
public class ContractDTO
    {
    public string? ContractName { get; set; }
    public int ClientID { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ContractStatus Status { get; set; }

    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    }
}