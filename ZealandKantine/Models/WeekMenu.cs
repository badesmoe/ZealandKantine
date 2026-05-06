#nullable disable
namespace ZealandKantine.Models
{
    public class WeekMenu
    {
        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public bool IsPublished { get; set; }
        public DateTime EmailSentAt { get; set; }
       
        public List<DailySpecial> DailySpecials { get; set; }

        public WeekMenu() { }
    }
}
