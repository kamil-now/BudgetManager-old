namespace BudgetManager.Application.Features.BudgetManagement;

public record UserSettingsRequest(string UserId) : IBudgetRequest, IRequest<UserSettingsDto>;
