using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly MonthlyStatementRepository _monthlyStatementRepository;
        private readonly CafeZea _dbContext;

        public OrderService(OrderRepository orderRepository, MonthlyStatementRepository monthlyStatementRepository, CafeZea dbContext)
        {
            _orderRepository = orderRepository;
            _monthlyStatementRepository = monthlyStatementRepository;
            _dbContext = dbContext;
        }

        private const int MenuItemDiscountPercent = 10;
        public void CreateOrder(Order order)
        {
            order.Status = "Modtaget";

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
        public void UpdateOrderStatus(int orderId, string status)
        {
            _orderRepository.UpdateStatus(orderId, status);
        }
        public List<Order> GetOrdersByUserId(int userId)
        {
            return _orderRepository.GetOrdersByUserId(userId);
        }

        public List<Order> GetCompletedOrdersFiltered(
         string? employeeName,
         string? period)
        {
            return _orderRepository
                .GetCompletedOrdersFiltered(
                    employeeName,
                    period);
        }

        public List<Order> ReadAll()
        {
            return _orderRepository.ReadAll();
        }

        public List<Order> GetOrdersByName(string? name)
        {
            return _orderRepository.GetOrdersByName(name);
        }

        public void GenerateMonthlyStatements(int month, int year)
        {
            var totals = _orderRepository.GetMonthlyTotalPerUser(month, year);

            foreach (var (userId, total) in totals)
            {
                if (_monthlyStatementRepository.ExistsForUserAndMonth(userId, month, year))
                    continue;

                var statement = new MonthlyStatement
                {
                    Userid = userId,
                    Month = month,
                    Year = year,
                    TotalAmount = total,
                    GeneratedAt = DateTime.Now
                };

                _monthlyStatementRepository.Create(statement);
            }
        }

    }
}
