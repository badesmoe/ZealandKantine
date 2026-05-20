using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Orders;

[Authorize(Roles = "Admin")]
public class OrderHistoryModel : PageModel
{
    private readonly OrderService _orderService;
    public List<Order> Orders { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? EmployeeSearch { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? PeriodFilter { get; set; }

    public OrderHistoryModel(OrderService orderService)
    {
        _orderService = orderService;
    }

    public void OnGet()    
    {
        if (!string.IsNullOrWhiteSpace(EmployeeSearch))
        {
            Orders = _orderService.GetOrdersByName(EmployeeSearch);
        }
        else
        {
            Orders = _orderService.ReadAll();
        }
    }
}