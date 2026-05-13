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

        public List<WeekMenu> GetCurrentWeek()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int daysFromMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
            var startOfWeek = today.AddDays(-daysFromMonday);
            var endOfWeek = startOfWeek.AddDays(5);

            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.DailySpecials)
                .Where(w => w.MenuDays.Any(md => md.Date >= startOfWeek && md.Date < endOfWeek))
                .ToList();
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
