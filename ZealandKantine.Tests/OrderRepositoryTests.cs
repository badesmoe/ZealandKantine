using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        [TestMethod]
        public void GetLatestOrderByUserId_ReturnsLatestOrder()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CafeZea>()
                .UseInMemoryDatabase(databaseName: "OrderTestDatabase")
                .Options;

            using (var context = new CafeZea(options))
            {
                context.Users.Add(new User { Id = 1, Name = "Simon", Email = "simon@bade.dk", Role = "Employee", Password = "test" });
                context.Orders.Add(new Order { Id = 1, UserId = 1, OrderDateTime = DateTime.Now.AddHours(-2), ReadyAt = DateTime.Now.AddHours(-1), Status = "Created" });
                context.Orders.Add(new Order { Id = 2, UserId = 1, OrderDateTime = DateTime.Now, ReadyAt = DateTime.Now.AddHours(1), Status = "Created" });
                context.Orders.Add(new Order { Id = 3, UserId = 2, OrderDateTime = DateTime.Now, ReadyAt = DateTime.Now.AddHours(1), Status = "Created" });
                context.SaveChanges();
            }

            using (var context = new CafeZea(options))
            {
                var repository = new OrderRepository(context);
                // Act
                var latestOrder = repository.GetLatestOrderByUserId(1);
                // Assert
                Assert.IsNotNull(latestOrder);
                Assert.AreEqual(2, latestOrder.Id);
                Assert.AreEqual(1, latestOrder.UserId);
            }
        }
    }
}
