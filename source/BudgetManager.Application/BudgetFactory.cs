public class BudgetFactory : IBudgetFactory
{
  public BudgetEntity Create(string userId, string defaultFundName)
  {
    return new BudgetEntity()
    {
      UserId = userId,
      Accounts = new List<AccountEntity>(),
      Funds = new List<FundEntity>() {
        new FundEntity() {
          Id = Guid.NewGuid().ToString(),
          Name = defaultFundName,
          IsDefault = true,
          Balance = new Dictionary<string, decimal>()
        }
      },
      FundTransfers = new List<FundTransferEntity>(),
      Incomes = new List<IncomeEntity>(),
      Expenses = new List<ExpenseEntity>(),
    };
  }
}