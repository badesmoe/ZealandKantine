using ZealandKantine.Models;
using Microsoft.EntityFrameworkCore;
using ZealandKantine.Interfaces;

namespace ZealandKantine.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly CafeZea _dbContext;

        public OrderRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }
        public void Create(Order entity)
        {
            _dbContext.Orders.Add(entity);
            _dbContext.SaveChanges();
        }
        public List<Order> ReadAll()
        {
            return _dbContext.Orders
                .Include(o => o.User)

                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.MenuItem)

                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.DailySpecial)

                .OrderByDescending(o => o.OrderDateTime)
                .ToList();
        }
        public Order? Read(int id)
        {
            return _dbContext.Orders.Find(id);
        }
        public void Update(Order entity)
        {
            var order = _dbContext.Orders.Find(entity.Id);
            if (order != null)
            {
                order.UserId = entity.UserId;
                order.OrderDateTime = entity.OrderDateTime;
                order.ReadyAt = entity.ReadyAt;
                order.Status = entity.Status;
                _dbContext.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            var order = _dbContext.Orders.Find(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                _dbContext.SaveChanges();
            }
        }

        public Order? GetLatestOrderByUserId(int userId)
        {
            return _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.MenuItem)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.DailySpecial)
                .OrderByDescending(o => o.OrderDateTime)
                .FirstOrDefault(o => o.UserId == userId);
        }
        public void UpdateStatus(int orderId, string status)
        {
            var order = _dbContext.Orders.Find(orderId);

            if (order != null)
            {
                order.Status = status;
                _dbContext.SaveChanges();
            }
        }

        public List<Order> GetActiveOrders()
        {
            return _dbContext.Orders
                .Where(o => o.Status != "Afhentet")
                .OrderBy(o => o.OrderDateTime)
                .ToList();
        }
        public List<Order> GetCompletedOrders()
        {
            return _dbContext.Orders
                .Where(o => o.Status == "Afhentet")
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();
        }
    }
}
