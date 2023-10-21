namespace BudgetManager.Application.Features.BudgetManagement;

public record AllocationRequest(string UserId, string AllocationId) : IBudgetRequest, IRequest<AllocationDto>;
