#nullable disable
namespace ZealandKantine.Models
{
    public enum MenuCategory
    {
        Sandwich,
        Snack,
        Drink,
        Other
    }
    public class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public MenuCategory Category { get; set; }
        public bool IsActive { get; set; }
    }
}
