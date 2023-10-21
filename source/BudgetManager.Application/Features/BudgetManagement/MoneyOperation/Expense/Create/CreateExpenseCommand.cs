namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateExpenseCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string Date,
  string AccountId,
  string FundId,
  string? Description
  ) : IRequest<string>, IBudgetCommand;
