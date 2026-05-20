using ZealandKantine.Models;

namespace ZealandKantine.Services
{
    public class EmailService
    {
		private readonly UserService _userService;

        public EmailService(UserService userService)
        {
            _userService = userService;
        }

        public void SendWeekMenu()
		{

			List<User> employees = _userService.GetUsers().Where(u => u.Role == "Employee").ToList();

			foreach (User e in employees)
			{
				TrySendEmail(e.Email, "Ugemenu", "Næste uges menu er nu klar");
			}
		}

        public void SendUpdatedWeekMenu()
        {

            List<User> employees = _userService.GetUsers().Where(u => u.Role == "Employee").ToList();

            foreach (User e in employees)
            {
                TrySendEmail(e.Email, "Opdateret Ugemenu", "Næste uges menu er ændret");
            }
        }

        public void SendDeletedOrder(Order deletedOrder)
        {
            List<User> employees = _userService.GetUsers()
                .Where(u => u.Orders.Any(o => o.Id == deletedOrder.Id))
                .ToList();

            foreach(User e in employees)
            {
                TrySendEmail(e.Email, "Ordre er blevet sletted", $"Din ordre er blevet sletted");
            }
        }

        private void TrySendEmail(string toAddress, string subject, string body)
        {

			try
			{
                Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine($"Email sendt to {toAddress}\n Subject: {subject}\n {body}");
                Console.ResetColor();
			}
			catch (Exception e)
			{
				Console.WriteLine($"An Error has occurd!\n {e}");
				throw;
			}
        }
    }
}
