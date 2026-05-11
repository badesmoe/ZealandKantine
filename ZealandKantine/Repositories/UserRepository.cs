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

        public int? GetUserIdByName(string name)
        {
            if (name != null)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Name == name);
                return user?.Id;
            }
            return 0;
        }
    }
}
