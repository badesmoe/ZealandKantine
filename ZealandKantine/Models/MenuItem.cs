#nullable disable
namespace ZealandKantine.Models
{
    public enum MenuCategory
    {
        Sandwich,
        Snack,
        Drink
    }
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public MenuCategory Category { get; set; }
        public bool IsActive { get; set; }

        public MenuItem() { }
    }
}
