using ZealandKantine.Repositories;
using ZealandKantine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;

namespace ZealandKantine.Tests
{
    [TestClass]
    public class MenuItemRepositoryTests
    {
        [TestMethod]
        public void GetActiveMenuItems_ReturnsOnlyActiveItems()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CafeZea>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CafeZea(options))
            {
                context.MenuItems.Add(new MenuItem { Id = 1, Name = "Active Item 1", Price = 10.00m, Category = MenuCategory.Morgenmad, IsActive = true });
                context.MenuItems.Add(new MenuItem { Id = 2, Name = "Inactive Item", Price = 5.00m, Category = MenuCategory.Frokost, IsActive = false });
                context.MenuItems.Add(new MenuItem { Id = 3, Name = "Active Item 2", Price = 7.50m, Category = MenuCategory.Drikkevarer, IsActive = true });
                context.SaveChanges();
            }

            using (var context = new CafeZea(options))
            {
                var repository = new MenuItemRepository(context);
                // Act
                var activeMenuItems = repository.GetAllActive();
                // Assert
                Assert.AreEqual(2, activeMenuItems.Count);
                Assert.IsTrue(activeMenuItems.All(item => item.IsActive));
            }
        }

        [TestMethod]
        public void GetTodaysDailySpecial_ReturnsCorrectSpecial()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CafeZea>()
                .UseInMemoryDatabase(databaseName: "DailySpecialTestDatabase")
                .Options;
            using (var context = new CafeZea(options))
            {
                context.DailySpecials.Add(new DailySpecial { Id = 1, Description = "Today's Special", Price = 15.00m, Date = DateTime.Today, IsActive = true });
                context.DailySpecials.Add(new DailySpecial { Id = 2, Description = "Tomorrow's Special", Price = 12.00m, Date = DateTime.Today.AddDays(1), IsActive = true });
                context.SaveChanges();
            }
            using (var context = new CafeZea(options))
            {
                var repository = new MenuItemRepository(context);
                // Act
                var todaysSpecial = repository.GetTodaysDailySpecial();
                // Assert
                Assert.IsNotNull(todaysSpecial);
                Assert.AreEqual("Today's Special", todaysSpecial.Description);
            }
        }
    }
}