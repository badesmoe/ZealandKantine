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

        //public List<MenuItem> GetAllMenuItems()
        //{
        //    return _menuItemRepository.GetAllMenuItems();
        //}

        //public List<MenuItem> GetTodaysSpecial()
        //{
        //    return _menuItemRepository.GetTodaysSpecial();
        //}
    }
}
