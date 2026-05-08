using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class MenuService
    {
        private readonly MenuItemRepository _menuItemRepository;

        public MenuService(MenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public void Create (MenuItem entiy)
        {
            _menuItemRepository.Create(entiy);
        }

        public List<MenuItem> GetAllActive()
        {
            return _menuItemRepository.GetAllActive();
        }

        public DailySpecial? GetTodaysDailySpecial()
        {
            return _menuItemRepository.GetTodaysDailySpecial();
        }
    }
}
