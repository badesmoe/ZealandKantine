using Microsoft.EntityFrameworkCore;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class WeekMenuRepository
    {
        private readonly CafeZea _dbContext;

        public WeekMenuRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(WeekMenu weekMenu, List<MenuDay> menuDays)
        {
            weekMenu.MenuDays = menuDays;
            weekMenu.EmailSentAt = DateTime.Now;

            _dbContext.WeekMenus.Add(weekMenu);
            _dbContext.SaveChanges();
        }

        public List<WeekMenu> GetAll()
        {
            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.DailySpecials)
                .OrderByDescending(w => w.Year)
                    .ThenByDescending(w => w.WeekNumber)
                .ToList();
        }
    }
}
