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
        private readonly MonthlyStatementRepository _monthlyStatementRepository;
        private readonly UserService _userService;

        public List<MonthlyStatement> Statements { get; set; }

        public MyStatementsModel(MonthlyStatementRepository monthlyStatementRepository, UserService userService)
        {
            _monthlyStatementRepository = monthlyStatementRepository;
            _userService = userService;
        }

        public void OnGet()
        {
            int userId = _userService.GetUserIdByName(User.Identity.Name) ?? 0;
            Statements = _monthlyStatementRepository.GetByUserId(userId);
        }
    }
}