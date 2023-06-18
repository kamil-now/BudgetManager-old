public class BudgetFactory : IBudgetFactory
{
  public BudgetEntity Create(string userId)
  {
    return new BudgetEntity()
    {
      UserId = userId,
      UserSettings = new UserSettingsEntity() {
        AccountsOrder = new List<string>(),
        FundsOrder = new List<string>(),
      },
      Accounts = new List<AccountEntity>(),
      Funds = new List<FundEntity>(),
      FundTransfers = new List<FundTransferEntity>(),
      Incomes = new List<IncomeEntity>(),
      Expenses = new List<ExpenseEntity>(),
    };
  }
}