using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Services;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.MonthlyStatements
{
    [IgnoreAntiforgeryToken]
    public class GenerateMonthlyStatement : PageModel
    {
        private readonly OrderService _orderService;
        private readonly MonthlyStatementService _monthlyStatementService;

        public GenerateMonthlyStatement(OrderService orderService, MonthlyStatementService monthlyStatementService)
        {
            _orderService = orderService;
            _monthlyStatementService = monthlyStatementService;
        }
        public IActionResult OnPost()
        {
            DateTime now = DateTime.Now;
            _monthlyStatementService.GenerateMonthlyStatements(now.Month, now.Year);
            return new OkResult();
        }
    }
}
