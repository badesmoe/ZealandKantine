using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Analysis
{
    public class IndexModel : PageModel
    {
        private readonly AnalysisService _analysisService;

        public List<(MenuItem MenuItem, int TotalSold)> SoldMenuItems { get; set; }

        public IndexModel(AnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public void OnGet()
        {
            SoldMenuItems = _analysisService.GetMenuItemsSortedBySales();
        }
    }
}
