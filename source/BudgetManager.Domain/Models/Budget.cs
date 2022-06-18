namespace BudgetManager.Domain.Models;

public class Budget
{
  public string? Id { get; init; }
  public IEnumerable<Account> Accounts { get; }
  public IEnumerable<Fund> Funds { get; }
  public SpendingFund SpendingFund { get; }
  private IEnumerable<MoneyOperation> Operations { get; }

  public Budget(
    SpendingFund spendingFund,
    IEnumerable<Account> accounts,
    IEnumerable<Fund> funds,
    IEnumerable<MoneyOperation> operations
    )
  {
    SpendingFund = spendingFund;
    Accounts = accounts;
    Funds = funds;
    Operations = operations;

    foreach (var operation in Operations)
    {
      ApplyOperation(operation);
    }
  }

  public void AddOperation<T>(T operation) where T : MoneyOperation
  {
    Operations.Append(operation);
    ApplyOperation(operation);
  }

  public Balance GetUnallocatedFunds()
  {
    var balance = new Balance();

    foreach (var account in Accounts)
    {
      balance.Add(account.Balance);
    }
    foreach (var fundMoney in Funds.SelectMany(x => x.Balance.Keys.Select(key => new Money(x.Balance[key], key))))
    {
      balance.Deduct(fundMoney);
    }

    return balance;
  }

  private void ApplyOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Allocation op:
        Funds.First(x => x.Id == op.FundId).Add(operation.Value);
        break;
      case Expense op:
        Funds.First(x => x.Id == op.FundId).Deduct(operation.Value);
        Accounts.First(x => x.Id == op.AccountId).Deduct(operation.Value);
        break;
      case Income op:
        Accounts.First(x => x.Id == op.AccountId).Add(operation.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }

  private IEnumerable<T> Get<T>() where T : MoneyOperation => Operations.Where(x => x is T).Select(x => (T)x);
}