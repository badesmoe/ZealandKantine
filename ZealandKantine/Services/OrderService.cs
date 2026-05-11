using Microsoft.EntityFrameworkCore;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly CafeZea _dbContext;

        public OrderService(OrderRepository orderRepository, CafeZea dbContext)
        {
            _orderRepository = orderRepository;
            _dbContext = dbContext;
        }

        private const int MenuItemDiscountPercent = 10;
        public void CreateOrder(Order order)
        {
            foreach (var item in order.OrderLines)
            {
                if (item.MenuItemId.HasValue)
                    item.MenuItem = _dbContext.MenuItems.Find(item.MenuItemId.Value);
                if (item.DailySpecialId.HasValue)
                    item.DailySpecial = _dbContext.DailySpecials.Find(item.DailySpecialId.Value);
                item.UnitPrice = item.MenuItem?.Price ?? item.DailySpecial?.Price ?? 0;

                decimal lineTotal = item.UnitPrice * item.Quantity;
                order.GrossTotal += lineTotal;

                bool isDiscountable = (item.MenuItem != null
                    && item.MenuItem.Category != MenuCategory.Drikkevarer) || item.DailySpecial != null;

                if (isDiscountable)
                {
                    item.DiscountPercent = MenuItemDiscountPercent;
                    item.DiscountAmount = lineTotal * MenuItemDiscountPercent / 100;
                    order.DiscountTotal += item.DiscountAmount;
                }
            }

            order.NetTotal = order.GrossTotal - order.DiscountTotal;
            _orderRepository.Create(order);
        }

        public Order? GetLatestOrderByUserId(int userId)
        {
            return _orderRepository.GetLatestOrderByUserId(userId);
        }
    }
}
