using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;

namespace ZealandKantine.Pages.MonthlyStatements
{
    [Authorize]
    public class MyStatementsModel : PageModel
    {
        private readonly MonthlyStatementRepository _monthlyStatementRepository;

        public List<MonthlyStatement> Statements { get; set; }

        public MyStatementsModel(MonthlyStatementRepository monthlyStatementRepository)
        {
            _monthlyStatementRepository = monthlyStatementRepository;
        }

        public void OnGet()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            Statements = _monthlyStatementRepository.GetByUserId(userId);
        }
    }
}