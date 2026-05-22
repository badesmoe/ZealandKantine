using System.Net;
using System.Net.Mail;
using ZealandKantine.Helpers;
using ZealandKantine.Models;
using System.Globalization;

namespace ZealandKantine.Services
{
    public class EmailService
    {

        // SMTP Configuration
        private static string _smtpServer = "smtp.simply.com";
        private static int _smtpPort = 587;  // Using TLS port
        private static string _username = EmailData.GetEmailName();
        private static string _password = EmailData.GetEmailPassword();

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

            //string body = $"Uge {weekMenu.WeekNumber} menu:\n\n";
            //    string body = $"<!DOCTYPE html>\r\n<html lang=\"da\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <title>Uge Menukort – Café Zea</title>\r\n</head>\r\n<body style=\"margin:0; padding:0; background-color:#f4f4f4; font-family:Arial, sans-serif;\">\r\n\r\n  <table role=\"presentation\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"background-color:#f4f4f4;\">\r\n    <tr>\r\n      <td align=\"center\" style=\"padding:30px 10px;\">\r\n\r\n        <!-- Container -->\r\n        <table role=\"presentation\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"\r\n          style=\"max-width:600px; width:100%; background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);\">\r\n\r\n          <!-- Header -->\r\n          <tr>\r\n            <td align=\"center\" style=\"background-color:#2c6e49; padding:30px 40px;\">\r\n              <h1 style=\"margin:0; color:#ffffff; font-size:26px; font-weight:bold; letter-spacing:1px;\">\r\n                Uge Menukort\r\n              </h1>\r\n              <p style=\"margin:8px 0 0; color:#a8d5b5; font-size:14px;\">\r\n                Café Zea – Uge 21, 2026\r\n              </p>\r\n            </td>\r\n          </tr>\r\n          <!-- Footer -->\r\n          <tr>\r\n            <td align=\"center\" style=\"background-color:#f9f9f9; padding:20px 40px; border-top:1px solid #e8e8e8;\">\r\n              <p style=\"margin:0; font-size:12px; color:#aaaaaa;\">\r\n                © 2026 – Café Zea / ZealandKantine\r\n              </p>\r\n              <p style=\"margin:6px 0 0; font-size:12px;\">\r\n                <a href=\"https://cafezea.dk/WeekMenus\" style=\"color:#2c6e49; text-decoration:none;\">\r\n                  Se menuen online\r\n                </a>\r\n              </p>\r\n            </td>\r\n          </tr>\r\n\r\n        </table>\r\n        <!-- /Container -->\r\n\r\n      </td>\r\n    </tr>\r\n  </table>\r\n\r\n</body>\r\n</html>";
            //    foreach (var day in weekMenu.MenuDays.OrderBy(d => d.DayOfWeek))
            //    {
            //        string dayName = day.DayOfWeek switch
            //        {
            //            1 => "Mandag",
            //            2 => "Tirsdag",
            //            3 => "Onsdag",
            //            4 => "Torsdag",
            //            5 => "Fredag",
            //            _ => "Ukendt"
            //        };

            //        body += $"{dayName}:\n";

            //        foreach (var mds in day.MenuDaySpecials)
            //        {
            //            body += $"  - {mds.DailySpecial.Description} ({mds.DailySpecial.Price} kr.)\n";
            //        }

            //        body += "\n";
            //    }

            //    foreach (User e in employees)
            //    {
            //        TrySendEmail(e.Email, $"Ugemenu for {weekMenu.WeekNumber} er ude nu", body);
            //    }
            //}

            string body = $@"
<!DOCTYPE html>
<html lang=""da"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Uge Menukort – Café Zea</title>
</head>
<body style=""margin:0; padding:0; background-color:#f4f4f4; font-family:Arial, sans-serif;"">
  <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color:#f4f4f4;"">
    <tr>
      <td align=""center"" style=""padding:30px 10px;"">
        <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""0"" border=""0""
          style=""max-width:600px; width:100%; background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);"">

          <!-- Header -->
          <tr>
            <td align=""center"" style=""background-color:#2c6e49; padding:30px 40px;"">
            <p style=""margin:0 0 8px; color:#a8d5b5; font-size:14px; text-transform:uppercase; letter-spacing:1px;"">
                Se dagens ret for næste uge:
              </p>
              <h1 style=""margin:0; color:#ffffff; font-size:26px; font-weight:bold; letter-spacing:1px;"">Uge Menukort</h1>
              <p style=""margin:8px 0 0; color:#a8d5b5; font-size:14px;"">
                Café Zea – Uge {weekMenu.WeekNumber}, {DateTime.Now.Year}
              </p>
            </td>
          </tr>

          <!-- Menu dage -->
          <tr>
            <td style=""padding:30px 40px;"">
";

            var days = weekMenu.MenuDays.OrderBy(d => d.DayOfWeek).ToList();

            for (int i = 0; i < days.Count; i++)
            {
                var day = days[i];
                var isLast = i == days.Count - 1;
                var borderStyle = isLast ? "" : "border-bottom:1px solid #e8e8e8;";

                string dayName = day.DayOfWeek switch
                {
                    1 => "Mandag",
                    2 => "Tirsdag",
                    3 => "Onsdag",
                    4 => "Torsdag",
                    5 => "Fredag",
                    _ => "Ukendt"
                };

                foreach (var mds in day.MenuDaySpecials)
                {
                    body += $@"
              <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0""
                style=""margin-bottom:16px; {borderStyle}"">
                <tr>
                  <td style=""padding-bottom:12px;"">
                    <h2 style=""margin:0 0 4px; font-size:16px; font-weight:bold; color:#2c6e49; text-transform:uppercase; letter-spacing:0.5px;"">
                      {dayName}
                    </h2>
                    <p style=""margin:0 0 4px; font-size:15px; color:#333333;"">
                      {mds.DailySpecial.Description}
                    </p>
                    <p style=""margin:0; font-size:14px; color:#888888; font-weight:bold;"">
                      {mds.DailySpecial.Price} kr.
                    </p>
                  </td>
                </tr>
              </table>
";
                }
            }

            body += $@"
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td align=""center"" style=""background-color:#f9f9f9; padding:20px 40px; border-top:1px solid #e8e8e8;"">
              <p style=""margin:0; font-size:12px; color:#aaaaaa;"">© {DateTime.Now.Year} – Café Zea / ZealandKantine</p>
              <p style=""margin:6px 0 0; font-size:12px;"">
                <a href=""https://cafezea.dk/WeekMenus"" style=""color:#2c6e49; text-decoration:none;"">Se menuen online</a>
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>
";

            foreach (User e in employees)
            {
                TrySendEmail(e.Email, $"Ugemenu for uge {weekMenu.WeekNumber} er ude nu", body);
            }
        }
        

        public void SendUpdatedWeekMenu()
        {
            List<User> employees = _userService.GetUsers().Where(u => u.Role == "Employee").ToList();
            WeekMenu weekMenu = _weekMenuService.GetCurrentWeek().FirstOrDefault();

            if (weekMenu == null)
                return;

            string body = $@"
<!DOCTYPE html>
<html lang=""da"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Uge Menukort – Café Zea</title>
</head>
<body style=""margin:0; padding:0; background-color:#f4f4f4; font-family:Arial, sans-serif;"">
  <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color:#f4f4f4;"">
    <tr>
      <td align=""center"" style=""padding:30px 10px;"">
        <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""0"" border=""0""
          style=""max-width:600px; width:100%; background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);"">

          <!-- Header -->
          <tr>
            <td align=""center"" style=""background-color:#2c6e49; padding:30px 40px;"">
            <p style=""margin:0 0 8px; color:#a8d5b5; font-size:14px; text-transform:uppercase; letter-spacing:1px;"">
                Se den opdaterede menu:
              </p>
              <h1 style=""margin:0; color:#ffffff; font-size:26px; font-weight:bold; letter-spacing:1px;"">Uge Menukort</h1>
              <p style=""margin:8px 0 0; color:#a8d5b5; font-size:14px;"">
                Café Zea – Uge {weekMenu.WeekNumber}, {DateTime.Now.Year}
              </p>
            </td>
          </tr>

          <!-- Menu dage -->
          <tr>
            <td style=""padding:30px 40px;"">
";

            var days = weekMenu.MenuDays.OrderBy(d => d.DayOfWeek).ToList();

            for (int i = 0; i < days.Count; i++)
            {
                var day = days[i];
                var isLast = i == days.Count - 1;
                var borderStyle = isLast ? "" : "border-bottom:1px solid #e8e8e8;";

                string dayName = day.DayOfWeek switch
                {
                    1 => "Mandag",
                    2 => "Tirsdag",
                    3 => "Onsdag",
                    4 => "Torsdag",
                    5 => "Fredag",
                    _ => "Ukendt"
                };

                foreach (var mds in day.MenuDaySpecials)
                {
                    body += $@"
              <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0""
                style=""margin-bottom:16px; {borderStyle}"">
                <tr>
                  <td style=""padding-bottom:12px;"">
                    <h2 style=""margin:0 0 4px; font-size:16px; font-weight:bold; color:#2c6e49; text-transform:uppercase; letter-spacing:0.5px;"">
                      {dayName}
                    </h2>
                    <p style=""margin:0 0 4px; font-size:15px; color:#333333;"">
                      {mds.DailySpecial.Description}
                    </p>
                    <p style=""margin:0; font-size:14px; color:#888888; font-weight:bold;"">
                      {mds.DailySpecial.Price} kr.
                    </p>
                  </td>
                </tr>
              </table>
";
                }
            }

            body += $@"
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td align=""center"" style=""background-color:#f9f9f9; padding:20px 40px; border-top:1px solid #e8e8e8;"">
              <p style=""margin:0; font-size:12px; color:#aaaaaa;"">© {DateTime.Now.Year} – Café Zea / ZealandKantine</p>
              <p style=""margin:6px 0 0; font-size:12px;"">
                <a href=""https://cafezea.dk/WeekMenus"" style=""color:#2c6e49; text-decoration:none;"">Se menuen online</a>
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>
";
            foreach (User e in employees)
            {
                TrySendEmail(e.Email, $"Ugemenu er blevet opdateret (uge {weekMenu.WeekNumber}", body);
            }
        }

        public void SendDeletedOrder(Order deletedOrder)
        {
            List<User> employees = _userService.GetUsers()
                .Where(u => u.Orders.Any(o => o.Id == deletedOrder.Id))
                .ToList();

            foreach(User e in employees)
            {
                TrySendEmail(e.Email, "Ordre er blevet slettet", $"Din ordre er blevet slettet");
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
                    IsBodyHtml = true  // Set to true if your email contains HTML
                };

                mailMessage.To.Add(toAddress);

                // Send the email
                smtpClient.Send(mailMessage);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Email er sendt!");
                Console.ResetColor();
            }
			catch (Exception e)
			{
				Console.WriteLine($"Der opstod en fejl!\n\n {e}");
				throw;
			}
        }
    }
}
