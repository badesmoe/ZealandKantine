using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class AnalysisService
    {
        private readonly AnalysisRepository _analysesRepository;

        public AnalysisService(AnalysisRepository analysesRepository)
        {
            _analysesRepository = analysesRepository;
        }

        public List<(MenuItem MenuItem, int TotalSold)> GetMenuItemsSortedBySales()
        {
            return _analysesRepository.GetOrderLines()
                .Where(ol => ol.MenuItem != null)
                .GroupBy(ol => ol.MenuItem)
                .OrderByDescending(g => g.Sum(ol => ol.Quantity))
                .Select(g => (g.Key, g.Sum(ol => ol.Quantity)))
                .ToList();
        }

        public List<(DailySpecial DailySpecial, int TotalSold)> GetDailySpecialsSortedBySales()
        {
            return _analysesRepository.GetOrderLines()
                .Where(ol => ol.DailySpecial != null)
                .GroupBy(ol => ol.DailySpecial)
                .OrderByDescending(g => g.Sum(ol => ol.Quantity))
                .Select(g => (g.Key, g.Sum(ol => ol.Quantity)))
                .ToList();
        }
    }
}
