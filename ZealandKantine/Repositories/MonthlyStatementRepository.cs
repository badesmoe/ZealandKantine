using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class MonthlyStatementRepository
    {
        private readonly CafeZea _dbContext;

        public MonthlyStatementRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(MonthlyStatement entity)
        {
            _dbContext.MonthlyStatements.Add(entity);
            _dbContext.SaveChanges();
        }
        public List<MonthlyStatement> GetByUserId(int userId)
        {
            return _dbContext.MonthlyStatements
                .Where(s => s.Userid == userId)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToList();
        }

        public bool ExistsForUserAndMonth(int userId, int month, int year)
        {
            return _dbContext.MonthlyStatements
                .Any(s => s.Userid == userId && s.Month == month && s.Year == year);
        }
    }
}
