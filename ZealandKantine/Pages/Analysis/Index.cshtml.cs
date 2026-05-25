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
        public List<(DailySpecial DailySpecial, int TotalSold)> SoldDailySpecials { get; set; }
        public List<(DateTime Date, decimal TotalRevenue)> RevenueByDate { get; set; }

        public IndexModel(AnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public void OnGet()
        {
            SoldMenuItems = _analysisService.GetMenuItemsSortedBySales();
            SoldDailySpecials = _analysisService.GetDailySpecialsSortedBySales();
            //RevenueByDate = _analysisService.GetRevenueByDate();

        }
    }
}
