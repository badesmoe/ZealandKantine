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
    }
}
