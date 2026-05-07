#nullable disable
namespace ZealandKantine.Models
{
    public class DailySpecial
    {
        public int Id { get; set; }
        //public int WeekMenuId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //public WeekMenu WeekMenu { get; set; }

        public DailySpecial() { }
    }
}
