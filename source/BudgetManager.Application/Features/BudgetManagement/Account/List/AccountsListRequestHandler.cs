namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountsListRequestHandler(IUserBudgetRepository repo, IMapper map) : BudgetRequestHandler<BudgetRequest<AccountDto>, IEnumerable<AccountDto>>(repo, map)
{
  public override IEnumerable<AccountDto> Get(BudgetRequest<AccountDto> request, Budget budget)
  => budget.Accounts.Where(x => !x.IsDeleted).Select(x =>
    {
      Account account = new(x.Id, x.Name, x.InitialBalance);
      foreach (var operation in budget.Operations)
      {
        if (operation is Expense expense && expense.AccountId == account.Id)
        {
          account.Deduct(expense.Value);
        }
        else if (operation is Income income && income.AccountId == account.Id)
        {
          account.Add(income.Value);
        }
        else if (operation is AccountTransfer accountTransfer)
        {
          if (accountTransfer.SourceAccountId == account.Id)
          {
            account.Deduct(accountTransfer.Value);
          }
          else if (accountTransfer.TargetAccountId == account.Id)
          {
            account.Add(accountTransfer.Value);
          }
        }
        else if (operation is CurrencyExchange currencyExchange && currencyExchange.AccountId == account.Id)
        {
          account.Deduct(currencyExchange.Value);
          account.Add(currencyExchange.Result);
        }
      }
      return _mapper.Map<AccountDto>(account);
    });
}
