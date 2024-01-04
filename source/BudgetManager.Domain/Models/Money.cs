namespace BudgetManager.Domain.Models;

public record struct Money(decimal Amount, string Currency)
{
  public static Money operator +(Money a, Money b)
  {
    var sum = a.Amount + b.Amount;
    return new Money(sum, a.Currency);
  }

  public static Money operator -(Money a, Money b)
  {
    var difference = a.Amount - b.Amount;
    return new Money(difference, a.Currency);
  }
}
