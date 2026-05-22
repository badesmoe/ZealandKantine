using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ZealandKantine.Models;
using ZealandKantine.Pages.WeekMenus;
using ZealandKantine.Services;

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

            EmailService emailService = new(new UserService(new UserRepository(_dbContext)), new WeekMenuService(new WeekMenuRepository(_dbContext)));

            emailService.SendWeekMenu();
        }

        public void Update(int weekMenuId, List<MenuDayInput> menuDays)
        {
            var weekMenu = _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.MenuDaySpecials)
                .FirstOrDefault(w => w.Id == weekMenuId);

            if (weekMenu == null)
                return;

            foreach (var dayInput in menuDays)
            {
                var menuDay = weekMenu.MenuDays
                    .FirstOrDefault(d => d.Id == dayInput.Id);

                if (menuDay == null)
                    continue;

                menuDay.DayOfWeek = (byte)dayInput.DayOfWeek;

                // Remove unselected specials from join table
                var toRemove = menuDay.MenuDaySpecials
                    .Where(mds => dayInput.SelectedDailySpecialIds == null ||
                                  !dayInput.SelectedDailySpecialIds.Contains(mds.DailySpecialId))
                    .ToList();

                foreach (var mds in toRemove)
                    _dbContext.MenuDaySpecials.Remove(mds);

                // Add newly selected specials to join table
                var existingIds = menuDay.MenuDaySpecials.Select(mds => mds.DailySpecialId).ToList();
                var toAdd = (dayInput.SelectedDailySpecialIds ?? new List<int>())
                    .Where(id => !existingIds.Contains(id))
                    .Select(id => new MenuDaySpecial { MenuDayId = menuDay.Id, DailySpecialId = id });

                _dbContext.MenuDaySpecials.AddRange(toAdd);
            }

            _dbContext.SaveChanges();

            EmailService emailService = new(new UserService(new UserRepository(_dbContext)), new WeekMenuService(new WeekMenuRepository(_dbContext)));
            emailService.SendUpdatedWeekMenu();
        }

        public List<WeekMenu> GetCurrentWeek()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int daysFromMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
            var startOfWeek = today.AddDays(-daysFromMonday);
            var endOfWeek = startOfWeek.AddDays(5);
            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.MenuDaySpecials)
                        .ThenInclude(mds => mds.DailySpecial)
                .Where(w => w.MenuDays.Any(md => md.Date >= startOfWeek && md.Date < endOfWeek))
                .ToList();
        }


        public WeekMenu? GetWeekMenu(int? weekNumber, int year)
        {
            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.MenuDaySpecials)
                        .ThenInclude(mds => mds.DailySpecial)
                .FirstOrDefault(w => w.WeekNumber == weekNumber && w.Year == year);
        }

        public List<WeekMenu> GetAll()
        {
            return _dbContext.WeekMenus
                .Include(w => w.MenuDays)
                    .ThenInclude(d => d.MenuDaySpecials)
                        .ThenInclude(mds => mds.DailySpecial)
                .OrderByDescending(w => w.Year)
                    .ThenByDescending(w => w.WeekNumber)
                .ToList();
        }
    }
}
