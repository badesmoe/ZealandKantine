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
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime ReadyAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal GrossTotal => OrderLines?.Sum(ol => ol.UnitPrice * ol.Quantity) ?? 0;
        public decimal DiscountTotal => OrderLines?.Sum(ol => ol.DiscountAmount) ?? 0;
        public decimal NetTotal => GrossTotal - DiscountTotal;

        public User User { get; set; }
        public List<OrderLine> OrderLines { get; set; }

        public Order() { }

        public Order(DateTime orderDateTime, DateTime readyAt, OrderStatus status)
        {
            OrderDateTime = orderDateTime;
            ReadyAt = readyAt;
            Status = status;
        }
    }
}
