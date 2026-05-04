#nullable disable
namespace ZealandKantine.Models
{
    public class OrderLine
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
        public DailySpecial DailySpecial { get; set; }
    }
}
