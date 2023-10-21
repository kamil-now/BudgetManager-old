namespace BudgetManager.Application.Features.BudgetManagement;

public record FundRequest(string UserId, string FundId) : IBudgetRequest, IRequest<FundDto>;
