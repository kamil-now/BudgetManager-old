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
   => budget.Accounts.Where(x => !x.IsDeleted).Select(x => _mapper.Map<AccountDto>(x));
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
