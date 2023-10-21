namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateAccountCommand(string UserId, string AccountId, string Name, Balance InitialBalance) : IRequest<AccountDto>, IBudgetCommand;
