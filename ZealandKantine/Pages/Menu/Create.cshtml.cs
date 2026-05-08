using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.Menu;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly MenuItemRepository _repository;

    //Constructor
    public CreateModel(MenuItemRepository repository)
    {
        _repository = repository;
    }

    [BindProperty]
    public MenuItem MenuItem { get; set; }
    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _repository.Create(MenuItem);

        return RedirectToPage("/Menu/Index");
    }

}

