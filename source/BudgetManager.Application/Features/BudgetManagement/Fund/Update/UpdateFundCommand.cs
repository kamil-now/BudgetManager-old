namespace BudgetManager.Application.Features.BudgetManagement;

public record UpdateFundCommand(string UserId, string FundId, string Name) : IRequest<FundDto>, IBudgetCommand;
