public record BalanceDto(Dictionary<string, decimal> currencyAmounts);
public record BalanceRequest(string UserId) : IRequest<BalanceDto>;

public class BalanceRequestHandler : IRequestHandler<BalanceRequest, BalanceDto>
{
  private IUserBudgetRepository _repository;
  public BalanceRequestHandler(IUserBudgetRepository repository) => _repository = repository;
  public Task<BalanceDto> Handle(BalanceRequest request, CancellationToken cancellationToken)
    => Task.FromResult(new BalanceDto(new Dictionary<string, decimal>()));
}
