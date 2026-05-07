using System.Data;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class MenuItemRepository
    {
        private readonly CafeZea _dbContext;

        public MenuItemRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }
        public List<MenuItem> GetAllActive()
        {
            var order = new List<MenuCategory>
            {
                MenuCategory.Morgenmad,
                MenuCategory.Frokost,
                MenuCategory.Drikkevarer
            };

            return _dbContext.MenuItems
                .Where(m => m.IsActive)
                .AsEnumerable()
                .OrderBy(m => order.IndexOf(m.Category))
                .ToList();
        }
        public DailySpecial? GetTodaysDailySpecial()
        {
            var today = DateTime.Today;
            return _dbContext.DailySpecials.FirstOrDefault(ds => ds.Date == today && ds.IsActive);
        }
    }
}
