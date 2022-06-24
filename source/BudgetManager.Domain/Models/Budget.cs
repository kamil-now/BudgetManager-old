using System.Collections.ObjectModel;

namespace BudgetManager.Domain.Models;

public class Budget
{
  public string? Id { get; init; }
  public SpendingFund SpendingFund { get; }
  public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
  public IReadOnlyCollection<Fund> Funds => _funds.AsReadOnly();
  public IReadOnlyCollection<MoneyOperation> Operations => _operations.AsReadOnly();
  private List<MoneyOperation> _operations;
  public List<Account> _accounts;
  public List<Fund> _funds;

  public Budget(
    SpendingFund spendingFund,
    IEnumerable<Account> accounts,
    IEnumerable<Fund> funds,
    IEnumerable<MoneyOperation> operations
    )
  {
    SpendingFund = spendingFund;
    _accounts = accounts.ToList();
    _funds = funds.ToList();
    _operations = operations.ToList();

    foreach (var operation in Operations)
    {
      ApplyOperation(operation);
    }
  }

  public void AddOperation<T>(T operation) where T : MoneyOperation
  {
    _operations.Add(operation);
    ApplyOperation(operation);
  }

  public void AddAccount(Account account) => _accounts.Add(account);
  public void RemoveAccount(Account account) => _accounts.Remove(account);
  public void AddFund(Fund fund) => _funds.Add(fund);
  public void RemoveFund(Fund fund) => _funds.Remove(fund);

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