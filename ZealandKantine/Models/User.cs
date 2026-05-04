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
        public string EmployeeId { get; set; }
        public UserRole Role { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; }
        public List<MonthlyStatement> MonthlyStatements { get; set; }

        public User() { }
        public User(string employeeId, string name, string email, UserRole role)
        {
            EmployeeId = employeeId;
            Name = name;
            Email = email;
            Role = role;
        }
    }

    public class MonthlyStatement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime GeneratedAt { get; set; }

        public User User { get; set; }

        public MonthlyStatement() { }
    }
}
