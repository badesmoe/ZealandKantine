using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? VerifyUser(string name, string password)
        {
            return _userRepository.VerifyUser(name, password);
        }

        public int ? GetUserIdByName(string name)
        {
            return _userRepository.GetUserIdByName(name);
        }
    }
}
