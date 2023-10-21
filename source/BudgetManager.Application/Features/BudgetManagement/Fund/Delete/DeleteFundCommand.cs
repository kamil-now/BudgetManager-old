namespace BudgetManager.Application.Features.BudgetManagement;

public record DeleteFundCommand(string UserId, string FundId) : IRequest<Unit>, IBudgetCommand;
