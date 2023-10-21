namespace BudgetManager.Application.Features.BudgetManagement;

public record DeleteAccountCommand(string UserId, string AccountId) : IRequest<Unit>, IBudgetCommand;
