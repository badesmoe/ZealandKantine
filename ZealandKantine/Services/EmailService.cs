using System.Net;
using System.Net.Mail;
using ZealandKantine.Helpers;
using ZealandKantine.Models;

namespace ZealandKantine.Services
{
    public class EmailService
    {

        // SMTP Configuration
        private static string _smtpServer = "smtp.simply.com";
        private static int _smtpPort = 587;  // Using TLS port
        private static string _username = EmaiData.GetEmailName();
        private static string _password = EmaiData.GetEmailPassword();

        private readonly UserService _userService;
        private readonly WeekMenuService _weekMenuService;

        public EmailService(UserService userService, WeekMenuService weekMenuService)
        {
            _userService = userService;
            _weekMenuService = weekMenuService;
        }

        public void SendWeekMenu()
		{

            List<User> employees = _userService.GetUsers().Where(u => u.Role == "Employee").ToList();
            WeekMenu weekMenu = _weekMenuService.GetAll().FirstOrDefault();

            if (weekMenu == null)
                return;

            string body = $"Uge {weekMenu.WeekNumber} menu:\n\n";

            foreach (var day in weekMenu.MenuDays.OrderBy(d => d.DayOfWeek))
            {
                string dayName = day.DayOfWeek switch
                {
                    1 => "Mandag",
                    2 => "Tirsdag",
                    3 => "Onsdag",
                    4 => "Torsdag",
                    5 => "Fredag",
                    _ => "Ukendt"
                };

                body += $"{dayName}:\n";

                foreach (var mds in day.MenuDaySpecials)
                {
                    body += $"  - {mds.DailySpecial.Description} ({mds.DailySpecial.Price} kr.)\n";
                }

                body += "\n";
            }

            foreach (User e in employees)
            {
                TrySendEmail(e.Email, $"Ugemenu for {weekMenu.WeekNumber} er ude nu", body);
            }
        }

        public void SendUpdatedWeekMenu()
        {
            List<User> employees = _userService.GetUsers().Where(u => u.Role == "Employee").ToList();
            WeekMenu weekMenu = _weekMenuService.GetCurrentWeek().FirstOrDefault();

            if (weekMenu == null)
                return;

            string body = $"<h1>Vi har opdateret ugemenuen for {weekMenu.WeekNumber}<h1/>\n Se menuen her:\n\n";

            foreach (var day in weekMenu.MenuDays.OrderBy(d => d.DayOfWeek))
            {
                string dayName = day.DayOfWeek switch
                {
                    1 => "Mandag",
                    2 => "Tirsdag",
                    3 => "Onsdag",
                    4 => "Torsdag",
                    5 => "Fredag",
                    _ => "Ukendt"
                };

                body += $"{dayName}:\n";

                foreach (var mds in day.MenuDaySpecials)
                {
                    body += $"  - {mds.DailySpecial.Description} ({mds.DailySpecial.Price} kr.)\n";
                }

                body += "\n";
            }

            foreach (User e in employees)
            {
                TrySendEmail(e.Email, "Opdateret Ugemenu", body);
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
                // Create the SMTP client
                SmtpClient smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true  // Use SSL/TLS
                };

                // Create the email message
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(_username),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false  // Set to true if your email contains HTML
                };

                mailMessage.To.Add(toAddress);

                // Send the email
                smtpClient.Send(mailMessage);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Email sent successfully!");
                Console.ResetColor();
            }
			catch (Exception e)
			{
				Console.WriteLine($"An Error has occurd!\n\n {e}");
				throw;
			}
        }
    }
}
