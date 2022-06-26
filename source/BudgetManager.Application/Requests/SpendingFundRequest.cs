namespace BudgetManager.Application.Requests;

using AutoMapper;

public record SpendingFundRequest(string UserId) : IBudgetRequest, IRequest<SpendingFundDto>;
public record SpendingFundDto(string Id, string Name, Balance Balance, Dictionary<string, Balance> Categories);

public class SpendingFundRequestHandler : BudgetRequestHandler<SpendingFundRequest, SpendingFundDto>
{
  public SpendingFundRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override SpendingFundDto Get(SpendingFundRequest request, Budget budget)
    => _mapper.Map<SpendingFundDto>(budget.SpendingFund);
}

public class SpendingFundRequestValidator : AbstractValidator<BalanceRequest>
{
  public SpendingFundRequestValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
        .WithMessage("Budget does not exists");
  }
}
