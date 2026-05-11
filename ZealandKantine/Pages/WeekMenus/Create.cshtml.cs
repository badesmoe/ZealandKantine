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
        public List<int> SelectedDailySpecialIds { get; set; } = new();

        public void OnGet()
        {
            var dailySpecials = _menuService.GetAllDailySpecials();
            DailySpecialsSelectList = new SelectList(dailySpecials, "Id", "Description");
        }

        public IActionResult OnPost()
        {
            if (SelectedDailySpecialIds.Count != 5)
            {
                ModelState.AddModelError("SelectedDailySpecialIds", "Du skal vælge præcis 5 dagens retter.");
            }

            if (!ModelState.IsValid)
            {
                DailySpecialsSelectList = new SelectList(_menuService.GetAllDailySpecials(), "Id", "Description");
                return Page();
            }

            var selectedSpecials = _menuService.GetAllDailySpecials()
                .Where(ds => SelectedDailySpecialIds.Contains(ds.Id))
                .ToList();

            WeekMenu.DailySpecials = selectedSpecials;
            _weekMenuService.Create(WeekMenu);

            DailySpecialsSelectList = new SelectList(_menuService.GetAllDailySpecials(), "Id", "Description");
            return Page();
        }
    }
}