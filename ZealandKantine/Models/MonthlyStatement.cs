#nullable disable
namespace ZealandKantine.Models
{
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
