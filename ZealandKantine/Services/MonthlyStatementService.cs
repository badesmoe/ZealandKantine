using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Services
{
    public class MonthlyStatementService
    {
        private readonly MonthlyStatementRepository _repository;
        private readonly OrderRepository _orderRepository;
        private readonly MonthlyStatementRepository _monthlyStatementRepository;

        public MonthlyStatementService(MonthlyStatementRepository repository, OrderRepository orderRepository, MonthlyStatementRepository monthlyStatementRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _monthlyStatementRepository = monthlyStatementRepository;
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

        public void GenerateMonthlyStatements(int month, int year)
        {
            var totals = _orderRepository.GetMonthlyTotalPerUser(month, year);

            foreach (var (userId, total) in totals)
            {
                if (_monthlyStatementRepository.ExistsForUserAndMonth(userId, month, year))
                    continue;

                var statement = new MonthlyStatement
                {
                    Userid = userId,
                    Month = month,
                    Year = year,
                    TotalAmount = total,
                    GeneratedAt = DateTime.Now
                };

                _monthlyStatementRepository.Create(statement);
            }
        }
    }
}