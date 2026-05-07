#nullable disable
namespace ZealandKantine.Models
{
    public enum UserRole
    {
        Admin,
        CanteenStaff,
        Employee
    }
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; }
        public List<MonthlyStatement> MonthlyStatements { get; set; }

        public User() { }
        public User(string name, string email, UserRole role)
        {
          
            Name = name;
            Email = email;
            Role = role;
        }
    }
}
