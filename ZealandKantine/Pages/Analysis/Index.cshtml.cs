using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.Analysis
{
    public class IndexModel : PageModel
    {
        private readonly AnalysisService _analysisService;

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-1);
        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Today;
        [BindProperty(SupportsGet = true)]
        public int? Quarter { get; set; }

        public List<(MenuItem MenuItem, int TotalSold)> SoldMenuItems { get; set; }
        public List<(DailySpecial DailySpecial, int TotalSold)> SoldDailySpecials { get; set; }
        public List<(DateTime Date, decimal TotalRevenue)> RevenueByDate { get; set; }

        public decimal TotalRevenue => RevenueByDate.Sum(r => r.TotalRevenue);

        public IndexModel(AnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public void OnGet()
        {
            if (Quarter.HasValue)
            {
                var year = DateTime.Today.Year;
                StartDate = Quarter.Value switch
                {
                    1 => new DateTime(year, 1, 1),
                    2 => new DateTime(year, 4, 1),
                    3 => new DateTime(year, 7, 1),
                    4 => new DateTime(year, 10, 1),
                    _ => StartDate
                };
                EndDate = Quarter.Value switch
                {
                    1 => new DateTime(year, 3, 31),
                    2 => new DateTime(year, 6, 30),
                    3 => new DateTime(year, 9, 30),
                    4 => new DateTime(year, 12, 31),
                    _ => EndDate
                };
            }
            SoldMenuItems = _analysisService.GetMenuItemsSortedBySales();
            SoldDailySpecials = _analysisService.GetDailySpecialsSortedBySales();
            RevenueByDate = _analysisService.GetRevenueByDate(StartDate, EndDate);

        }
    }
}
