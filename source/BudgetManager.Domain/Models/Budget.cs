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
  }

  public void AddOperation<T>(T operation) where T : MoneyOperation
  {
    ApplyOperation(operation);
    _operations.Add(operation);
  }

  public void RemoveOperation<T>(string operationId) where T : MoneyOperation
  {
    var operation = _operations.First(x => x.Id == operationId);
    UndoOperation(operation);
    _operations.RemoveAll(x => x.Id == operationId);
  }

  public string AddAccount(string accountName, Money initialBalance)
  {
    var id = Guid.NewGuid().ToString();
    _accounts.Add(new Account(id, accountName, initialBalance));
    SpendingFund.Add(initialBalance);
    return id;
  }
  public void RenameAccount(string accountId, string newName) => _accounts.First(x => x.Id == accountId).Name = newName;
  public void RemoveAccount(string accountId)
  {
    var account = _accounts.First(x => x.Id == accountId);
    _accounts.RemoveAll(x => x.Id == accountId);
    SpendingFund.Deduct(account.InitialBalance);
  }

  public string AddFund(string name)
  {
    var id = Guid.NewGuid().ToString();
    _funds.Add(new Fund(id, name));
    return id;
  }
  public void RenameFund(string fundId, string newName) => _funds.First(x => x.Id == fundId).Name = newName;
  public void RemoveFund(string fundId) => _funds.RemoveAll(x => x.Id == fundId);

  public void AddSpendingCategory(string categoryName) => SpendingFund.Categories.Add(categoryName, new Balance());
  public void UpdateSpendingCategory(string oldName, string newName)
  {
    var value = SpendingFund.Categories[oldName];
    RemoveSpendingCategory(oldName);
    SpendingFund.Categories[newName] = value;
  }
  public void RemoveSpendingCategory(string categoryName) => SpendingFund.Categories.Remove(categoryName);

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

  // TODO refactor
  private void ApplyOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Allocation op:
        if (op.FundId is not null)
        {
          _funds.First(x => x.Id == op.FundId).Add(operation.Value);
        }
        else if (op.Category is not null)
        {
          if (!SpendingFund.Categories.ContainsKey(op.Category))
          {
            SpendingFund.Categories.Add(op.Category, new Balance());
          }

          SpendingFund.Categories[op.Category].Add(op.Value);
        }
        SpendingFund.Deduct(operation.Value);
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
        SpendingFund.Add(operation.Value);
        break;
      case FundTransfer op:
        _funds.First(x => x.Id == op.SourceFundId).Deduct(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Add(op.Value);
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
        SpendingFund.Add(operation.Value);
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
        SpendingFund.Deduct(operation.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }
}