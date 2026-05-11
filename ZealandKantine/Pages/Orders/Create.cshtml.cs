using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Orders
{
    [Authorize (Roles = "Admin, Employee")]
    public class CreateModel : PageModel
    {
        private readonly MenuService _menuService;
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public CreateModel(MenuService menuService, OrderService orderService, UserService userService)
        {
            _menuService = menuService;
            _orderService = orderService;
            _userService = userService;
        }
        public List<MenuItem>? MenuItems { get; set; }
        public DailySpecial? TodaysDailySpecial { get; set; }

        [BindProperty]
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
        public void OnGet()
        {
            MenuItems = _menuService.GetAllActive();
            TodaysDailySpecial = _menuService.GetTodaysDailySpecial();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            Order order = new Order
            {
                UserId = _userService.GetUserIdByName(User.Identity.Name) ?? 0,
                OrderDateTime = DateTime.Now,
                ReadyAt = DateTime.Now.AddMinutes(60),
                Status = "Created",
                OrderLines = OrderLines
            };

            _orderService.CreateOrder(order);

            return RedirectToPage("/Orders/Confirmation");
        }
    }
}
