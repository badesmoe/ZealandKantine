using Microsoft.AspNetCore.Identity;
using ZealandKantine.Interfaces;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? VerifyUser(string name, string password)
        {
            User? user = _userRepository.Read(name);

            if (user == null)
                return null;

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success ? user : null;
        }

        public int ? GetUserIdByName(string name)
        {
            return _userRepository.GetUserIdByName(name);
        }
    }
}
