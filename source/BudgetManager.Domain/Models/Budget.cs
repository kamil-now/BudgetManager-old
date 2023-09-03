namespace BudgetManager.Domain.Models;

public class Budget
{
  public string? Id { get; init; }
  public UserSettings UserSettings => _userSettings;
  public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
  public IReadOnlyCollection<Fund> Funds => _funds.AsReadOnly();
  public IReadOnlyCollection<MoneyOperation> Operations => _operations.AsReadOnly();

  private List<MoneyOperation> _operations;
  private List<Account> _accounts;
  private List<Fund> _funds;
  private UserSettings _userSettings;

  public Budget(
    UserSettings userSettings,
    IEnumerable<Account> accounts,
    IEnumerable<Fund> funds,
    IEnumerable<MoneyOperation> operations
    )
  {
    _userSettings = userSettings;
    _accounts = accounts.ToList();
    _funds = funds.ToList();
    _operations = operations.ToList();
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

  public string AddAccount(string accountName, string? fundId, Money initialBalance)
  {
    var id = Guid.NewGuid().ToString();
    var account = new Account(id, accountName, initialBalance.Currency);
    account.Add(initialBalance);
    _accounts.Add(account);
    if (initialBalance.Amount > 0)
    {
      _funds.First(x => x.Id == fundId).Add(initialBalance);
    }
    return id;
  }
  public Account RenameAccount(string accountId, string newName)
  {
    var account = _accounts.First(x => x.Id == accountId);
    account.Name = newName;
    return account;
  }
  public void RemoveAccount(string accountId)
  {
    var account = _accounts.First(x => x.Id == accountId);
    if (account.Balance.Amount > 0)
    {
      throw new InvalidOperationException("Account cannot be deleted if its balance is greater than 0");
    }
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
    if (fund.Balance.Values.Any(x => x > 0))
    {
      throw new InvalidOperationException("Fund cannot be deleted if its balance is greater than 0");
    }
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
        _funds.First(x => x.Id == op.FundId).Add(operation.Value);
        break;
      case FundTransfer op:
        _funds.First(x => x.Id == op.SourceFundId).Deduct(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Add(op.Value);
        break;
      case AccountTransfer op:
        _accounts.First(x => x.Id == op.SourceAccountId).Deduct(op.Value);
        _accounts.First(x => x.Id == op.TargetAccountId).Add(op.Value);
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
        _funds.First(x => x.Id == op.FundId).Deduct(operation.Value);
        break;
      case FundTransfer op:
        _funds.First(x => x.Id == op.SourceFundId).Add(op.Value);
        _funds.First(x => x.Id == op.TargetFundId).Deduct(op.Value);
        break;
      case AccountTransfer op:
        _accounts.First(x => x.Id == op.SourceAccountId).Add(op.Value);
        _accounts.First(x => x.Id == op.TargetAccountId).Deduct(op.Value);
        break;
      default:
        throw new InvalidOperationException("Unhandled operation");
    }
  }
}