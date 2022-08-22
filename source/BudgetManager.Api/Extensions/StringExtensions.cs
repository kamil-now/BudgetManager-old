namespace BudgetManager.Api;

public static class StringExtensions
{
  public static string SplitCamelCase(this string input)
  {
    return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
  }

  public static string CapitalizeFirstLetter(this string input)
  {
    return input.Substring(0, 1).ToUpper() + input.Substring(1);
  }
}