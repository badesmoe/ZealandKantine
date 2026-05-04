#nullable disable
namespace ZealandKantine.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? MenuItemId { get; set; }
        public int? DailySpecialId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal => (UnitPrice * Quantity) - DiscountAmount;

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
        public DailySpecial DailySpecial { get; set; }
    }
}
