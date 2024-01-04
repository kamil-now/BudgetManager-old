namespace BudgetManager.Application.Extensions;

internal sealed class RequestValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

  public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var context = new ValidationContext<TRequest>(request);

    var validationErrors = _validators
        .Select(x => x.ValidateAsync(context).Result)
        .SelectMany(x => x.Errors)
        .ToArray();

    return validationErrors.Any() ? throw new ValidationException("One or more validation errors: " + string.Join(" ", validationErrors.Select(x => x.ErrorMessage).ToArray()), validationErrors) : next();
  }
}
