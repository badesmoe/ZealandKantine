using ZealandKantine.Models;
using ZealandKantine.Pages.WeekMenus;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class WeekMenuService
    {
        private readonly WeekMenuRepository _weekMenuRepository;

        public WeekMenuService(WeekMenuRepository weekMenuRepository)
        {
            _weekMenuRepository = weekMenuRepository;
        }

        public void Create(WeekMenu weekMenu, List<MenuDay> menuDays)
        {
            _weekMenuRepository.Create(weekMenu, menuDays);
        }

        public void Update(int weekMenuId, List<MenuDayInput> menuDays)
        {
            _weekMenuRepository.Update(weekMenuId, menuDays);
        }

        public List<WeekMenu> GetCurrentWeek()
        {
            return _weekMenuRepository.GetCurrentWeek();
        }

        public WeekMenu? GetWeekMenu(int? weekNumber, int year)
        {
            return _weekMenuRepository.GetWeekMenu(weekNumber, year);
        }

        public List<WeekMenu> GetAll()
        {
            return _weekMenuRepository.GetAll();
        }
    }
}
