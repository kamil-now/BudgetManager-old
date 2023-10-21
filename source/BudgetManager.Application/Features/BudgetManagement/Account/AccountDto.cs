namespace BudgetManager.Application.Features.BudgetManagement;

public record AccountDto(
    string Id,
    string Name,
    Balance Balance,
    Balance InitialBalance,
    bool IsDeleted);
    