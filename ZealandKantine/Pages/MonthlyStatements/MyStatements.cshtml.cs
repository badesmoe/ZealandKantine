using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Models;
using ZealandKantine.Repositories;
using ZealandKantine.Services;

namespace ZealandKantine.Pages.MonthlyStatements
{
    [Authorize]
    public class MyStatementsModel : PageModel
    {
        private readonly MonthlyStatementService _statementService;
        private readonly UserService _userService;

        public List<MonthlyStatement> Statements { get; set; }
        public string? PeriodFilter { get; set; }

        public string ViewMode { get; set; } = "cards";

        public MyStatementsModel(MonthlyStatementService statementService, UserService userService)
        {
            _statementService = statementService;
            _userService = userService;
        }

        public void OnGet(string? periodFilter, string? viewMode)
        { 
          int userId = _userService.GetUserIdByName(User.Identity.Name) ?? 0;
          
            PeriodFilter = periodFilter;

            ViewMode = viewMode ?? "cards";

            Statements = _statementService.GetStatementsForUser(userId, periodFilter);
        }
           
    }
}