public class BudgetFactory : IBudgetFactory
{
  public BudgetEntity Create(string userId)
  {
    return new BudgetEntity()
    {
      UserId = userId,
      SpendingFund = new SpendingFundEntity()
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Spending Fund",
        Balance = new Dictionary<string, decimal>()
      }
    };
  }
}