using AutoMapper;

namespace BudgetManager.Application.Features.BudgetManagement;

public class BudgetFactory(IMapper _mapper) : IBudgetFactory
{
  const string DATE_FORMAT = "yyyy/MM/dd";
  public BudgetEntity Create(string userId)
  {
    return new BudgetEntity()
    {
      UserId = userId,
      UserSettings = new UserSettingsEntity()
      {
        AccountsOrder = new List<string>(),
        FundsOrder = new List<string>(),
      },
      Accounts = new List<AccountEntity>(),
      Funds = new List<FundEntity>(),
      FundTransfers = new List<FundTransferEntity>(),
      AccountTransfers = new List<AccountTransferEntity>(),
      Incomes = new List<IncomeEntity>(),
      Expenses = new List<ExpenseEntity>(),
      CurrencyExchanges = new List<CurrencyExchangeEntity>(),
      Allocations = new List<AllocationEntity>(),
    };
  }

  public BudgetEntity CreateWithSampleData(string userId)
  {
    var budgetEntity = Create(userId);
    var budget = _mapper.Map<Budget>(budgetEntity);
    AddSampleData(budget);
    var withSampleData = _mapper.Map<BudgetEntity>(budget);
    withSampleData.UserId = userId;
    return withSampleData;
  }

  private static void AddSampleData(Budget budget)
  {
    string currency = "EUR";
    var cashId = budget.AddAccount("Cash", new Balance(new Dictionary<string, decimal>() { { currency, 20 }, { "USD", 200 } }));
    var checkingAccountId = budget.AddAccount("Checking Account", new Balance());
    var savingsAccountId = budget.AddAccount("Savings Account", new Balance());

    var taxesFundId = budget.AddFund("Taxes");
    var rentFundId = budget.AddFund("Rent & Utilities");
    var transportFundId = budget.AddFund("Transport");
    var healthFundId = budget.AddFund("Health");
    var foodFundId = budget.AddFund("Food");
    var entertainmentFundId = budget.AddFund("Entertainment");
    var savingsFundId = budget.AddFund("Savings");
    var emergencyFundId = budget.AddFund("Emergency");

    var payday = DateTime.Now.AddDays(-7);

    Allocation CreateSalaryAllocation(decimal amount, string fundId)
    {
      payday = payday.AddSeconds(1);
      return CreateAllocation(fundId, "Allocate from salary", new Money(amount, currency), payday);
    }
    budget.AddOperations(new List<MoneyOperation>() {
      CreateIncome(checkingAccountId, "Salary", new Money(5000, currency), payday),

      CreateSalaryAllocation(1400, rentFundId),
      CreateSalaryAllocation(400, transportFundId),
      CreateSalaryAllocation(700, taxesFundId),
      CreateSalaryAllocation(1000, healthFundId),
      CreateSalaryAllocation(500, foodFundId),
      CreateSalaryAllocation(200, entertainmentFundId),
      CreateSalaryAllocation(300, savingsFundId),
      CreateSalaryAllocation(500, emergencyFundId),

      CreateAccountTransfer(checkingAccountId, savingsAccountId, "Transfer savings", new Money(800, currency), payday.AddSeconds(1)),

      CreateExpense(foodFundId, checkingAccountId, "Groceries", new Money(30, currency), payday.AddDays(1)),
      CreateExpense(transportFundId, checkingAccountId, "Uber", new Money(5, currency), payday.AddDays(2)),

      CreateAccountTransfer(checkingAccountId, cashId, "Cash withdrawal", new Money(100, currency), payday.AddDays(3)),

      CreateExpense(transportFundId, checkingAccountId, "Uber", new Money(5, currency), payday.AddDays(4)),
      CreateExpense(entertainmentFundId, cashId, "Night out", new Money(42, currency), payday.AddDays(4)),
      CreateIncome(cashId, "Night out reimbursement", new Money(10, currency), payday.AddDays(5)),
  });
  }

  private static Income CreateIncome(string accuntId, string title, Money money, DateTime date)
    => new(
        Guid.NewGuid().ToString(),
        accuntId,
        title,
        money,
        date.ToString(DATE_FORMAT),
        string.Empty,
        DateTime.Now);

  private static Allocation CreateAllocation(string fundId, string title, Money money, DateTime date)
    => new(
        Guid.NewGuid().ToString(),
        title,
        money,
        fundId,
        date.ToString(DATE_FORMAT),
        string.Empty,
        DateTime.Now);

  private static AccountTransfer CreateAccountTransfer(string sourceAccuntId, string targetAccountId, string title, Money money, DateTime date)
    => new(
        Guid.NewGuid().ToString(),
        title,
        money,
        sourceAccuntId,
        targetAccountId,
        date.ToString(DATE_FORMAT),
        string.Empty,
        DateTime.Now);

  private static Expense CreateExpense(string fundId, string accountId, string title, Money money, DateTime date)
    => new(
        Guid.NewGuid().ToString(),
        title,
        money,
        date.ToString(DATE_FORMAT),
        accountId,
        fundId,
        string.Empty,
        DateTime.Now);
}
