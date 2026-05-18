using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.WeekMenus
{
    [Authorize(Roles = "Admin")]
    public class UpdateModel : PageModel
    {
        private readonly WeekMenuService _weekMenuService;
        private readonly MenuService _menuService;

        public int SelectedYear { get; set; } = DateTime.Now.Year;
        public int? SelectedWeekNumber { get; set; }
        public WeekMenu? WeekMenu { get; set; }
        public List<MenuDayInput> MenuDays { get; set; } = new();
        public List<DailySpecial> AllDailySpecials { get; set; } = new();

        [BindProperty]
        public List<MenuDayInput> MenuDays_Post { get; set; } = new();

        [BindProperty]
        public int WeekMenuId { get; set; }

        public UpdateModel(WeekMenuService weekMenuService, MenuService menuService)
        {
            _weekMenuService = weekMenuService;
            _menuService = menuService;
        }

        public void OnGet(int? weekNumber, int? year)
        {
            SelectedWeekNumber = weekNumber;
            SelectedYear = year ?? DateTime.Now.Year;
            AllDailySpecials = _menuService.GetAllDailySpecials().ToList();
            WeekMenu = _weekMenuService.GetWeekMenu(SelectedWeekNumber, SelectedYear);

            if (WeekMenu != null)
            {
                MenuDays = WeekMenu.MenuDays
                    .OrderBy(d => d.DayOfWeek)
                    .Select(d => new MenuDayInput
                    {
                        Id = d.Id,
                        DayOfWeek = d.DayOfWeek,
                        SelectedDailySpecialIds = d.DailySpecials.Select(ds => ds.Id).ToList()
                    }).ToList();
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _weekMenuService.Update(WeekMenuId, MenuDays_Post);
            return RedirectToPage("/WeekMenus/Index");
        }
    }
}
