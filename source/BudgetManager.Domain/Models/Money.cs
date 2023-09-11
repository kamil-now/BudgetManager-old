namespace BudgetManager.Domain.Models;

// public readonly record struct Money(decimal Amount, string Currency);
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

  private void EnsureTheSameCurrency(Money a, Money b)
  {
    if (a.Currency != b.Currency)
    {
      throw new InvalidOperationException("Arithmetic operations on moneys with different currencies is not supported.");
    }
  }
}
