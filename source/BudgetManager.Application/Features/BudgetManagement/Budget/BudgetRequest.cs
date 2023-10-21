namespace BudgetManager.Application.Features.BudgetManagement;

public record BudgetRequest<TDto>(string UserId) : IBudgetRequest, IRequest<IEnumerable<TDto>>;
