using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Services;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.MonthlyStatements
{
    public class GenerateStatementModel : PageModel
    {
        private readonly OrderService _orderService;

        public GenerateStatementModel(OrderService orderService)
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
