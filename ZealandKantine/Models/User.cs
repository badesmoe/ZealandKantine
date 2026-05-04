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
        private static int _nextID = 0;
        public int EmployeeId { get; set; }
        public UserRole Role { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; }
        public List<MonthlyStatement> MonthlyStatements { get; set; }

        public User() { }
        public User(string name, string email, UserRole role)
        {
            _nextID++;
            EmployeeId = _nextID;
            Name = name;
            Email = email;
            Role = role;
        }
    }

    public class MonthlyStatement
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime GeneratedAt { get; set; }

        public User User { get; set; }

        public MonthlyStatement() { }
    }
}
