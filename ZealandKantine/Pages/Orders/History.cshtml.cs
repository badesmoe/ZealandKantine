using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Interfaces;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.Orders
{
    public class HistoryModel : PageModel
    {
        private readonly OrderRepository _orderRepository;

        public HistoryModel(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> Orders { get; set; } = new();
        public DateTime SelectedDate { get; set; }


        public void OnGet(DateTime? selectedDate)
        {
            SelectedDate = selectedDate ?? DateTime.Today;
            Orders = _orderRepository.ReadAll()
                .Where(o => o.Status == "Afhentet"
                         && o.OrderDateTime.Date == SelectedDate.Date)
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();
        }
    }
}