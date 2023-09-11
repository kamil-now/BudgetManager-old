namespace BudgetManager.Domain.Models;

public class Budget
{
  public string? Id { get; init; }
  public UserSettings UserSettings => _userSettings;
  public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
  public IReadOnlyCollection<Fund> Funds => _funds.AsReadOnly();
  public IReadOnlyCollection<MoneyOperation> Operations => _operations.AsReadOnly();
  public Balance Unallocated => _unallocated; // TODO make readonly

  private readonly List<MoneyOperation> _operations;
  private readonly List<Account> _accounts;
  private readonly List<Fund> _funds;
  private UserSettings _userSettings;
  private Balance _unallocated;

  public Budget(
    UserSettings userSettings,
    IEnumerable<Account> accounts,
    IEnumerable<Fund> funds,
    IEnumerable<MoneyOperation> operations,
    Balance unallocated
    )
  {
    _userSettings = userSettings;
    _accounts = accounts.ToList();
    _funds = funds.ToList();
    _operations = operations.ToList();
    _unallocated = unallocated;
  }

  public void UpdateUserSettings(IEnumerable<string> accountsOrder, IEnumerable<string> fundsOrder)
  {
    _userSettings = new UserSettings(accountsOrder, fundsOrder);
  }

  public T GetOperation<T>(string operationId) where T : MoneyOperation
  {
    if (Operations.First(x => x.Id == operationId) is not T operation)
    {
      throw new InvalidOperationException();
    }
    return operation;
  }

  public void AddOperation<T>(T operation) where T : MoneyOperation
  {
    ApplyOperation(operation);
    _operations.Add(operation);
  }

  public T UpdateOperation<T>(string operationId, Action<T> update) where T : MoneyOperation
  {
    var operation = GetOperation<T>(operationId);
    UndoOperation(operation);
    update(operation);
    ApplyOperation(operation);
    return operation;
  }

  public T RemoveOperation<T>(string operationId) where T : MoneyOperation
  {
    var operation = _operations.First(x => x.Id == operationId);
    UndoOperation(operation);
    _operations.RemoveAll(x => x.Id == operationId);

    return (T)operation;
  }

  public string AddAccount(string accountName, Balance initialBalance)
  {
    var id = Guid.NewGuid().ToString();
    var account = new Account(id, accountName, initialBalance);
    _unallocated.Add(initialBalance);
    _accounts.Add(account);
    return id;
  }

  public Account UpdateAccount(string accountId, string? name, Balance? initialBalance)
  {
    var account = _accounts.First(x => x.Id == accountId);
    if (name is not null)
    {
      account.Name = name;
    }

    if (initialBalance is not null)
    {
      account.Balance.Deduct(account.InitialBalance);
      _unallocated.Deduct(account.InitialBalance);

      account.InitialBalance = new Balance(initialBalance);

      account.Balance.Add(account.InitialBalance);
      _unallocated.Add(account.InitialBalance);

      var toRemove = _unallocated.Keys.Where(key => _unallocated[key] == 0);
      foreach (var key in toRemove)
      {
        _unallocated.Remove(key);
      }
    }
    return account;
  }
  public void RemoveAccount(string accountId)
  {
    var account = _accounts.First(x => x.Id == accountId);
    account.IsDeleted = true;
  }

  public string AddFund(string name)
  {
    var id = Guid.NewGuid().ToString();
    _funds.Add(new Fund(id, name));
    return id;
  }
  public Fund RenameFund(string fundId, string newName)
  {
    var fund = _funds.First(x => x.Id == fundId);
    fund.Name = newName;
    return fund;
  }
  public void RemoveFund(string fundId)
  {
    var fund = _funds.First(x => x.Id == fundId);
    fund.IsDeleted = true;
  }

  private void ApplyOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Expense op:
        _funds.First(x => x.Id == op.FundId).Deduct(op.Value);
        _accounts.First(x => x.Id == op.AccountId).Deduct(op.Value);
        break;
      case Income op:
        _accounts.First(x => x.Id == op.AccountId).Add(operation.Value);
        _unallocated.Add(operation.Value);
        break;
      case FundTransfer op:
        _funds.First(x => x.Id == op.SourceFundId).Deduct(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Add(op.Value);
        break;
      case AccountTransfer op:
        _accounts.First(x => x.Id == op.SourceAccountId).Deduct(op.Value);
        _accounts.First(x => x.Id == op.TargetAccountId).Add(op.Value);
        break;
      case Allocation op:
        _unallocated.Deduct(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Add(op.Value);
        break;
      case CurrencyExchange op:
        var account =_accounts.First(x => x.Id == op.AccountId);
        account.Deduct(op.Value);
        account.Add(op.Result);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }

  private void UndoOperation<T>(T operation) where T : MoneyOperation
  {
    switch (operation)
    {
      case Expense op:
        _funds.First(x => x.Id == op.FundId).Add(op.Value);
        _accounts.First(x => x.Id == op.AccountId).Add(op.Value);
        break;
      case Income op:
        _accounts.First(x => x.Id == op.AccountId).Deduct(operation.Value);
        _unallocated.Deduct(operation.Value);
        break;
      case FundTransfer op:
        _funds.First(x => x.Id == op.SourceFundId).Add(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Deduct(op.Value);
        break;
      case AccountTransfer op:
        _accounts.First(x => x.Id == op.SourceAccountId).Add(op.Value);
        _accounts.First(x => x.Id == op.TargetAccountId).Deduct(op.Value);
        break;
      case Allocation op:
        _unallocated.Add(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Deduct(op.Value);
        break;
      case CurrencyExchange op:
        var account =_accounts.First(x => x.Id == op.AccountId);
        account.Deduct(op.Result);
        account.Add(op.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }
}