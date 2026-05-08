using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Menu
{
    public class IndexModel : PageModel
    {
        private readonly MenuService _menuService;
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();
        public DailySpecial? TodaysSpecial { get; set; }
        public IndexModel(MenuService menuService)
        {
            _menuService = menuService;
        }
        public void OnGet()
        {
            Items = _menuService.GetAllActive();
            TodaysSpecial = _menuService.GetTodaysDailySpecial();
        }

        public IActionResult OnPostCreate()
        {
            return RedirectToPage("/Menu/Create");
        }
    }
}
