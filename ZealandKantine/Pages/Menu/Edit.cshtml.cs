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

    [BindProperty]
    public decimal? Price { get; set; }

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

    public IActionResult OnPostUpdate()
    {
        if (!ModelState.IsValid)
        {
            Items = _repository.ReadAll();
            return Page();
        }

        // Hvis prisen ikke er udfyldt, beholdes den gamle pris
        if (!Price.HasValue)
        {
            var existing = _repository.Read(MenuItem.Id);
            MenuItem.Price = existing.Price;
        }
        else
        {
            MenuItem.Price = Price.Value;
        }

        _repository.Update(MenuItem);
        Items = _repository.ReadAll();
        return Page();
    }

    public IActionResult OnPostDelete()
    {
        _repository.Delete(MenuItem.Id);
        Items = _repository.ReadAll();
        return Page();
    }
}