using Microsoft.AspNetCore.Identity;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class UserRepository : IVerifyUser
    {
        private readonly CafeZea _dbContext;

        public UserRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public User? VerifyUser(string name, string password)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.Name == name);

            if (user == null)
                return null;

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success ? user : null;
        }
    }
}
