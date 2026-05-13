using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.WeekMenus
{
    public class IndexModel : PageModel
    {
        private readonly WeekMenuService _weekMenuService;

        public IndexModel(WeekMenuService weekMenuService)
        {
            _weekMenuService = weekMenuService;
        }

        public List<WeekMenu> WeekMenu { get; set; } = new();

        public void OnGet()
        {
            WeekMenu = _weekMenuService.GetCurrentWeek();
        }
    }
}