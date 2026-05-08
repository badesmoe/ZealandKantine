using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public interface IVerifyUser
    {
        User? VerifyUser(string name, string password);
    }
}
