using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.DailySpecialPages;

public class CreateModel : PageModel
{
    private readonly DailySpecialRepository _repository;

    public CreateModel(DailySpecialRepository repository)
    {
        _repository = repository;
    }

    [BindProperty]
    public Models.DailySpecial DailySpecial { get; set; }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        DailySpecial.IsActive = true;

        _repository.Create(DailySpecial);

        return RedirectToPage("/Menu/Index");
    }
}