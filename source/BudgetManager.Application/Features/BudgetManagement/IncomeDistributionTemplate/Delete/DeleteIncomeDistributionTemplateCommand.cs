namespace BudgetManager.Application.Features.BudgetManagement;

public record DeleteIncomeDistributionTemplateCommand(string UserId, string IncomeDistributionTemplateId) : IRequest<Unit>, IBudgetCommand;