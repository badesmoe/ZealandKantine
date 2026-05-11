using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Services;
using ZealandKantine.Models;

namespace ZealandKantine.Pages.Orders
{
    public class ConfirmationModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public Order? Order { get; set; }

        public ConfirmationModel(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }
        public void OnGet()
        {
            int? userId = _userService.GetUserIdByName(User.Identity.Name);
            Order = _orderService.GetLatestOrderByUserId(userId ?? 0);
        }
    }
}
