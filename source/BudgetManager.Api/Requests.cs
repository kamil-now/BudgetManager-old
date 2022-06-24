
public record CreateAccountRequest(string Name, decimal InitialAmount, string Currency);
public record CreateFundRequest(string Name, Dictionary<string, decimal> InitialBalance);