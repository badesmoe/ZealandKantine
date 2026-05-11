using ZealandKantine.Models;

namespace ZealandKantine.Interfaces
{
    public interface IUserService
    {
        User? VerifyUser(string name, string password);
    }
}
