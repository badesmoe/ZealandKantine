using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.Orders
{
    public class ManageModel : PageModel
    {
        private readonly OrderRepository _repository;

        public List<Order> Orders { get; set; }

        public ManageModel(OrderRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Orders = _repository.ReadAll()
                .Where(o => o.Status != "Afhentet")
                .ToList();
        }

        public IActionResult OnPostUpdateStatus(int orderId, string status)
        {
            var order = _repository.Read(orderId);

            if (order != null)
            {
                order.Status = status;

                _repository.Update(order);
            }

            return RedirectToPage();
        }
    }
}