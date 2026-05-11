using ZealandKantine.Models;
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

        public void Create(WeekMenu weekMenu)
        {
            _weekMenuRepository.Create(weekMenu);
        }

    }
}
