namespace BudgetManager.Application.Extensions;

public static class FluentValidationExtensions
{
   public static IRuleBuilderOptions<T, string> ISO_4217_Currency<T>(this IRuleBuilder<T, string> ruleBuilder, bool allowNull = false)
    => ruleBuilder
       .Must(currency => currency is null && allowNull || ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == currency))
       .WithMessage("'Currency' must comply with ISO 4217.");

   public static IRuleBuilderOptions<T, Money?> ISO_4217_Currency<T>(this IRuleBuilder<T, Money?> ruleBuilder, bool allowNull = false)
    => ruleBuilder
       .Must(money => money is null && allowNull || money.HasValue && ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == money.Value.Currency))
       .WithMessage("'Currency' must comply with ISO 4217.");
}
