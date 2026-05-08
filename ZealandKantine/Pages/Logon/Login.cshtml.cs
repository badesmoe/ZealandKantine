using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using ZealandKantine.Models;
using ZealandKantine.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ZealandKantine.Pages.Logon
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _userService.VerifyUser(Name, Password);

            if (user == null)
            {
                // Handle invalid login
                ErrorMessage = "Upsi, dit brugernavn eller kodeord er forkert";
                return Page();
            }

            // Handle successful login
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            BuildClaimsPrincipal(user));

            return RedirectToPage("/Index");
        }

        // Don't know if this is the best way to do this, but it works for now. P.S. took it form the lecture slides, so it should be fine.
        private ClaimsPrincipal BuildClaimsPrincipal(User user)
        {
            // Opbyg Claims-liste
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.Name),
                              new Claim(ClaimTypes.Role, user.Role)];

            // Opret ClaimsIdentity (claims plus Authentication-strategi)
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Opret endeligt ClaimsPrincipal-objekt
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
