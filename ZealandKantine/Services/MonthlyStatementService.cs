using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class MonthlyStatementService
    {
        private readonly MonthlyStatementRepository _repository;

        public MonthlyStatementService(MonthlyStatementRepository repository)
        {
            _repository = repository;
        }

        public List<MonthlyStatement> GetStatementsForUser(int userId, string? periodFilter)
        {
            List<MonthlyStatement> statements = _repository.GetByUserId(userId);

            // Filter this month
            if (periodFilter == "thismonth")
            {
                statements = statements
                    .Where(s => s.Month == DateTime.Now.Month && s.Year == DateTime.Now.Year)
                    .ToList();
            }

            // Filter last month
            if (periodFilter == "lastmonth")
            {
                DateTime lastMonth = DateTime.Now.AddMonths(-1);

                statements = statements
                    .Where(s => s.Month == lastMonth.Month && s.Year == lastMonth.Year)
                    .ToList();
            }

            // Sort newest first
            statements = statements.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month)
                .ToList();

            return statements;
        }
    }
}