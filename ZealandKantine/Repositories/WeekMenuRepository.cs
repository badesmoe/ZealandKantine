using Microsoft.EntityFrameworkCore;
using ZealandKantine.Models;
using ZealandKantine.Pages.WeekMenus;

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

        public void Update(int weekMenuId, List<MenuDayInput> menuDays)
        {
            var weekMenu = _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.DailySpecials)
                .FirstOrDefault(w => w.Id == weekMenuId);

            if (weekMenu == null)
                return;

            foreach (var dayInput in menuDays)
            {
                var menuDay = weekMenu.MenuDays
                    .FirstOrDefault(d => d.Id == dayInput.Id);

                if (menuDay == null)
                    continue;

                // Update fields
                menuDay.DayOfWeek = (byte)dayInput.DayOfWeek;

                // Remove unselected specials
                foreach (var existing in menuDay.DailySpecials.ToList())
                {
                    if (dayInput.SelectedDailySpecialIds == null ||
                        !dayInput.SelectedDailySpecialIds.Contains(existing.Id))
                    {
                        existing.MenuDayId = null;
                    }
                }

                // Add selected specials
                var selectedSpecials = _dbContext.DailySpecials
                    .Where(ds => dayInput.SelectedDailySpecialIds.Contains(ds.Id))
                    .ToList();

                foreach (var special in selectedSpecials)
                {
                    special.MenuDayId = menuDay.Id;
                }
            }

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

        public WeekMenu? GetWeekMenu(int? weekNumber, int year)
        {
            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.DailySpecials)
                .FirstOrDefault(w => w.WeekNumber == weekNumber && w.Year == year);
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
