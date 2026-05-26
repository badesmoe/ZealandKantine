using Microsoft.EntityFrameworkCore;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class AnalysisRepository
    {
        private readonly CafeZea _dbContext;

        public AnalysisRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public List<OrderLine> GetOrderLines()
        {
            return _dbContext.OrderLines
                .Include(ol => ol.MenuItem)
                .Include(ol => ol.DailySpecial)
                .Include(ol => ol.Order)
                .ToList();
        }
    }
}
