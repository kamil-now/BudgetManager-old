namespace BudgetManager.Application.Requests;

using AutoMapper;

public record AccountTransferRequest(string UserId, string AccountTransferId) : IRequest<AccountTransferDto>, IBudgetRequest;

public class AccountTransferRequestHandler : BudgetRequestHandler<AccountTransferRequest, AccountTransferDto>,IRequestHandler<AccountTransferRequest, AccountTransferDto>
{
  public AccountTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AccountTransferDto Get(AccountTransferRequest request, Budget budget)
  {
    var accountTransfer = budget.Operations.First(x => x.Id == request.AccountTransferId) as AccountTransfer;
    return _mapper.Map<AccountTransferDto>(accountTransfer);
  }
}

public class AccountTransfersRequestHandler : BudgetRequestHandler<BudgetRequest<AccountTransferDto>, IEnumerable<AccountTransferDto>>
{
  public AccountTransfersRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<AccountTransferDto> Get(BudgetRequest<AccountTransferDto> request, Budget budget)
   => budget.Operations.Where(x => x is AccountTransfer).Select(x => _mapper.Map<AccountTransferDto>(x as AccountTransfer));
}

public class AccountTransferRequestValidator : BudgetRequestValidator<AccountTransferRequest>
{
  public AccountTransferRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.AccountTransfers?.Any(x => x.Id == request.AccountTransferId) ?? false;
      }).WithMessage("Account transfer with a given id does not exist in the budget.");
  }
}
