using Microsoft.AspNetCore.Identity;
using ZealandKantine.Interfaces;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class UserRepository
    {
        private readonly CafeZea _dbContext;

        public UserRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public User? Read(string name)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Name == name);
        }
    }
}
