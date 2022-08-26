namespace BudgetManager.Application.DependencyInjection;

internal sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public RequestValidationBehavior(
      IEnumerable<IValidator<TRequest>> validators) =>
      _validators = validators;

  public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
  {
    var context = new ValidationContext<TRequest>(request);

    var validationErrors = _validators
        .Select(x => x.ValidateAsync(context).Result)
        .SelectMany(x => x.Errors)
        .ToArray();

    return validationErrors.Any() ? throw new ValidationException("One or more validation errors: " + string.Join(" ", validationErrors.Select(x => x.ErrorMessage).ToArray()), validationErrors) : next();
  }
}