namespace BudgetManager.Application.Requests;

using AutoMapper;

public record AccountRequest(string UserId, string AccountId) : IBudgetRequest, IRequest<AccountDto>;

public class AccountRequestHandler : BudgetRequestHandler<AccountRequest, AccountDto>
{
  public AccountRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AccountDto Get(AccountRequest request, Budget budget)
  {
    var account = budget.Accounts.First(x => x.Id == request.AccountId);
    return _mapper.Map<AccountDto>(account);
  }
}

public class AccountsRequestHandler : BudgetRequestHandler<BudgetRequest<AccountDto>, IEnumerable<AccountDto>>
{
  public AccountsRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

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

public class AccountRequestValidator : BudgetRequestValidator<AccountRequest>
{
  public AccountRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Accounts?.Any(x => x.Id == request.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget.");
  }
}
