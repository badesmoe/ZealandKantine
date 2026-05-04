#nullable disable
namespace ZealandKantine.Models
{
    public enum OrderStatus
    {
        Created,
        Ready,
        PickedUp,
        Cancelled
    }
    public class Order
    {
        public DateTime OrderDateTime { get; set; }
        public DateTime ReadyAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal NetTotal { get; set; }

        public User User { get; set; }
        public List<OrderLine> OrderLines { get; set; }

        public Order() { }

        public Order(DateTime orderDateTime, DateTime readyAt, OrderStatus status, decimal grossTotal, decimal discountTotal, decimal netTotal)
        {
            OrderDateTime = orderDateTime;
            ReadyAt = readyAt;
            Status = status;
            GrossTotal = grossTotal;
            DiscountTotal = discountTotal;
            NetTotal = netTotal;
        }
    }
}
