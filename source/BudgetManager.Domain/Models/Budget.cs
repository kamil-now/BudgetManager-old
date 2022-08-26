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

    AssignUnallocatedFundsToSpendingFund();
  }

  public void AddOperation<T>(T operation) where T : MoneyOperation
  {
    _operations.Add(operation);
    ApplyOperation(operation);
  }

  public void RemoveOperation<T>(string operationId) where T : MoneyOperation
  {
    var operation = _operations.First(x => x.Id == operationId);
    UndoOperation(operation);
    _operations.RemoveAll(x => x.Id == operationId);
  }

  public void AddAccount(Account account) => _accounts.Add(account);
  public void RenameAccount(string accountId, string newName) => _accounts.First(x => x.Id == accountId).Name = newName;
  public void RemoveAccount(string accountId) => _accounts.RemoveAll(x => x.Id == accountId);

  public void AddFund(Fund fund) => _funds.Add(fund);
  public void RenameFund(string fundId, string newName) => _funds.First(x => x.Id == fundId).Name = newName;
  public void RemoveFund(string fundId) => _funds.RemoveAll(x => x.Id == fundId);

  public void ToggleExpenseIsConfirmed(string expenseId)
  {
    var expense = _operations.First(x => x.Id == expenseId) as Expense;
    expense!.IsConfirmed = !expense!.IsConfirmed;
    if (expense!.IsConfirmed)
    {
      ApplyOperation(expense);
    }
    else
    {
      UndoOperation<Expense>(expense);
    }
  }

  private void AssignUnallocatedFundsToSpendingFund()
  {
    var unallocatedFunds = GetUnallocatedFunds(false);

    foreach (var category in SpendingFund.Categories)
    {
      foreach (var (currency, amount) in category.Value)
      {
        unallocatedFunds.Deduct(new Money(amount, currency));
      }
    }
    foreach (var currency in unallocatedFunds.Keys)
    {
      SpendingFund.Add(new Money(unallocatedFunds[currency], currency));
    }
  }

  private Balance GetUnallocatedFunds(bool excludePlannedExpenses)
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

    if (excludePlannedExpenses)
    {
      foreach (var expense in Operations.Where(x => x is Expense e && e.Date > DateOnly.FromDateTime(DateTime.Now)))
      {
        balance.Deduct(expense.Value);
      }
    }

    return balance;
  }

  // TODO refactor
  private void ApplyOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Allocation op:
        _funds.First(x => x.Id == op.FundId).Add(operation.Value);
        break;
      case Expense op:
        if (op.Date <= DateOnly.FromDateTime(DateTime.Now) && op.IsConfirmed)
        {
          if (op.FundId is null)
          {
            if (op.Category is null)
            {
              SpendingFund.Deduct(operation.Value);
            }
            else
            {
              if (!SpendingFund.Categories.ContainsKey(op.Category))
              {
                SpendingFund.Categories.Add(op.Category, new Balance());
              }

              SpendingFund.Categories[op.Category].Deduct(op.Value);
            }
          }
          else
          {
            _funds.First(x => x.Id == op.FundId).Deduct(op.Value);
          }
          _accounts.First(x => x.Id == op.AccountId).Deduct(op.Value);
        }
        break;
      case Income op:
        _accounts.First(x => x.Id == op.AccountId).Add(operation.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }

  // TODO refactor
  private void UndoOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Allocation op:
        _funds.First(x => x.Id == op.FundId).Deduct(operation.Value);
        break;
      case Expense op:
        if (op.Date <= DateOnly.FromDateTime(DateTime.Now) && op.IsConfirmed)
        {
          if (op.FundId is null)
          {
            if (op.Category is null)
            {
              SpendingFund.Add(operation.Value);
            }
            else
            {
              if (!SpendingFund.Categories.ContainsKey(op.Category))
              {
                SpendingFund.Categories.Add(op.Category, new Balance());
              }

              SpendingFund.Categories[op.Category].Add(op.Value);
            }
          }
          else
          {
            _funds.First(x => x.Id == op.FundId).Add(op.Value);
          }
          _accounts.First(x => x.Id == op.AccountId).Add(op.Value);
        }
        break;
      case Income op:
        _accounts.First(x => x.Id == op.AccountId).Deduct(operation.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }
}