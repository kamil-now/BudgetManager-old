using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Api.BudgetController;

public class ExpenseController : Controller
{
  public IActionResult Get()
  {
    return Ok();
  }

  [HttpPost]
  public IActionResult Add()
  {
    return Ok();
  }

  [HttpPut]
  public IActionResult Update()
  {
    return Ok();
  }
}