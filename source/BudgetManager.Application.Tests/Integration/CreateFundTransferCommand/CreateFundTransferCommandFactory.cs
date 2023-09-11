using BudgetManager.Application.Commands;
using BudgetManager.Domain.Models;

namespace CreateFundTransferCommandTests;

public class CreateFundTransferCommandFactory
{
  public CreateFundTransferCommand CreateInvalidCommand(string userId)
    => new(
       userId,
       "mockFundTransfer",
       new Money(1, "EUR"),
       null,
       null,
       "",
       ""
     );
  public CreateFundTransferCommand CreateWithInvalidSourceFundId(string userId, string targetFundId)
      => new(
         userId,
         "mockFundTransfer",
         new Money(1, "EUR"),
         null,
         "",
         "invalid id",
         targetFundId
       );

  public CreateFundTransferCommand CreateWithInvalidTargetFundId(string userId, string sourceFundId)
    => new(
       userId,
       "mockFundTransfer",
       new Money(1, "EUR"),
       null,
       "",
       sourceFundId,
       "invalid id"
     );

  public CreateFundTransferCommand CreateWithExceededFunds(
    string userId,
    string sourceFundId,
    string targetFundId,
    Money money)
    => new(
       userId,
       "mockFundTransfer",
        money with { Amount = money.Amount + 0.00000000001m },
       "",
       "",
       sourceFundId,
       targetFundId
     );
}