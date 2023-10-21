namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateAccountTransferCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string Date,
  string? Description,
  string AccountId,
  string TargetAccountId
  ) : IRequest<string>, IBudgetCommand;
