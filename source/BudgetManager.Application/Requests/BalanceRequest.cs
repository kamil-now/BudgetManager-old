using FluentValidation;
using MediatR;

public record BalanceDto(Dictionary<string, decimal> currencyAmounts);
public record BalanceRequest(int UserId) : IRequest<BalanceDto>;

public class BalanceRequestHandler : IRequestHandler<BalanceRequest, BalanceDto>
{
  public Task<BalanceDto> Handle(BalanceRequest request, CancellationToken cancellationToken)
    => Task.FromResult(new BalanceDto(new Dictionary<string, decimal>()));
}

public class BalanceRequestValidator : AbstractValidator<BalanceRequest>
{
  public BalanceRequestValidator()
  {
    RuleFor(x => x.UserId)
        .Must(userId => userId > 20);
  }
}