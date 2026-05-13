using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.WeekMenus
{
    [Authorize(Roles = "Employee")]
    public class CreateModel : PageModel
    {
        private readonly WeekMenuService _weekMenuService;
        private readonly MenuService _menuService;

        public CreateModel(WeekMenuService weekMenuService, MenuService menuService)
        {
            _weekMenuService = weekMenuService;
            _menuService = menuService;
        }

        public SelectList DailySpecialsSelectList { get; set; }

        [BindProperty]
        public WeekMenu WeekMenu { get; set; }

        [BindProperty]
        public List<MenuDayInput> MenuDays { get; set; } = new();

        public void OnGet()
        {
            var dailySpecials = _menuService.GetAllDailySpecials();
            DailySpecialsSelectList = new SelectList(dailySpecials, "Id", "Description");
        }

        public IActionResult OnPost()
        {
            // Valider at der er valgt mindst én ret pr. dag
            foreach (var day in MenuDays)
            {
                if (day.SelectedDailySpecialIds == null || !day.SelectedDailySpecialIds.Any())
                {
                    ModelState.AddModelError(
                        "MenuDays",
                        $"Du skal vælge mindst én ret for {GetDayName(day.DayOfWeek)}."
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                DailySpecialsSelectList = new SelectList(_menuService.GetAllDailySpecials(), "Id", "Description");
                return Page();
            }

            // Byg MenuDay-objekter med tilhørende DailySpecials
            var menuDays = MenuDays.Select(d => new MenuDay
            {
                DayOfWeek = (byte)d.DayOfWeek,
                Date = GetDateForDay(WeekMenu.WeekNumber, WeekMenu.Year, d.DayOfWeek),
                DailySpecials = _menuService.GetAllDailySpecials()
                    .Where(ds => d.SelectedDailySpecialIds.Contains(ds.Id))
                    .ToList()
            }).ToList();

            _weekMenuService.Create(WeekMenu, menuDays);

            return RedirectToPage("./Index");
        }

        private string GetDayName(int dayOfWeek) => dayOfWeek switch
        {
            1 => "mandag",
            2 => "tirsdag",
            3 => "onsdag",
            4 => "torsdag",
            5 => "fredag",
            _ => "ukendt dag"
        };

        private DateOnly GetDateForDay(int weekNumber, int year, int dayOfWeek)
        {
            // Find første mandag i den givne uge
            var jan4 = new DateTime(year, 1, 4);
            var monday = jan4.AddDays(7 * (weekNumber - 1) - (int)jan4.DayOfWeek + 1);
            return DateOnly.FromDateTime(monday.AddDays(dayOfWeek - 1));
        }
    }

    public class MenuDayInput
    {
        public int DayOfWeek { get; set; }
        public List<int> SelectedDailySpecialIds { get; set; } = new();
    }
}