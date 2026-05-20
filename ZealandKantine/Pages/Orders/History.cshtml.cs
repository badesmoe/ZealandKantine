using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Orders
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PeriodFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ViewMode { get; set; }

        public List<Order> Orders { get; set; } = new();

        public HistoryModel(
            OrderService orderService,
            UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public void OnGet()
        {
            int userId =
                _userService.GetUserIdByName(User.Identity.Name) ?? 0;

            Orders = _orderService.GetOrdersByUserId(userId);

            // SEARCH
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Orders = Orders
                    .Where(o =>
                        o.Id.ToString().Contains(SearchTerm) ||
                        o.Status.Contains(SearchTerm))
                    .ToList();
            }

            // FILTER
            if (PeriodFilter == "thismonth")
            {
                Orders = Orders
                    .Where(o => o.OrderDateTime.Month == DateTime.Now.Month)
                    .ToList();
            }

            if (PeriodFilter == "lastmonth")
            {
                Orders = Orders
                    .Where(o =>
                        o.OrderDateTime.Month ==
                        DateTime.Now.AddMonths(-1).Month)
                    .ToList();
            }
        }
    }
}