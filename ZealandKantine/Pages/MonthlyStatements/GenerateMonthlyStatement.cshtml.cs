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

        public GenerateMonthlyStatement(OrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult OnPost()
        {
            DateTime now = DateTime.Now;
            _orderService.GenerateMonthlyStatements(now.Month, now.Year);
            return new OkResult();
        }
    }
}
