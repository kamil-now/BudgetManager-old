namespace BudgetManager.Application.Requests;

using AutoMapper;

public record FundTransferDto(
    string Id,
    string Title,
    Money Value,
    string Date,
    string Description,
    string? SourceFundId,
    string? TargetFundId
);
public record FundTransferRequest(string UserId, string FundTransferId) : IBudgetRequest, IRequest<FundTransferDto>;

public class FundTransferRequestHandler : BudgetRequestHandler<FundTransferRequest, FundTransferDto>
{
  public FundTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override FundTransferDto Get(FundTransferRequest request, Budget budget)
  {
    var FundTransfer = budget.Operations.First(x => x.Id == request.FundTransferId) as FundTransfer;
    return _mapper.Map<FundTransferDto>(FundTransfer);
  }
}

public class FundTransfersRequestHandler : BudgetRequestHandler<BudgetRequest<FundTransferDto>, IEnumerable<FundTransferDto>>
{
  public FundTransfersRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<FundTransferDto> Get(BudgetRequest<FundTransferDto> request, Budget budget)
   => budget.Operations.Where(x => x is FundTransfer).Select(x => _mapper.Map<FundTransferDto>(x as FundTransfer));
}

public class FundTransferRequestValidator : BudgetRequestValidator<FundTransferRequest>
{
  public FundTransferRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.FundTransfers?.Any(x => x.Id == request.FundTransferId) ?? false;
      }).WithMessage("FundTransfer with a given id does not exist in the budget");
  }
}
