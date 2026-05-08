using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandKantine.Models;
using ZealandKantine.Pages.Logon;
using ZealandKantine.Repositories;
using ZealandKantine.Services;

namespace ZealandKantine.Tests
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void TestLogin_GetAdminUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CafeZea>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var dbContext = new CafeZea(options);

            var passwordHasher = new PasswordHasher<User>();

            var testUser = new User
            {
                Name = "admin"
            };

            testUser.Password = passwordHasher.HashPassword(testUser, "admin");

            dbContext.Users.Add(testUser);
            dbContext.SaveChanges();

            var userRepository = new UserRepository(dbContext);
            var userService = new UserService(userRepository);

            // Act
            var user = userService.VerifyUser("admin", "admin");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("admin", user.Name);
        }
    }
}
