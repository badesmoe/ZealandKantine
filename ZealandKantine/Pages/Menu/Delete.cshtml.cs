using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.Menu;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly MenuItemRepository _repository;

    public DeleteModel(MenuItemRepository repository)
    {
        _repository = repository;
    }

    [BindProperty]
    public MenuItem MenuItem { get; set; }
    
    public List<MenuItem> Items { get; set; } = new();

    public void OnGet()
    {
        Items = _repository.ReadAll();
    }

    public IActionResult OnGetDelete(int id)
    {
        MenuItem = _repository.Read(id);
        if (MenuItem == null)
        {
            return NotFound();
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        //if (!ModelState.IsValid)
        //{
        //    Items = _repository.ReadAll();
        //    return Page();
        //}

        _repository.Delete(MenuItem.Id);
        return RedirectToPage("/Menu/Delete");
    }
}