using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using eczASP.Net.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace chtotonaASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly MusicStoreContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(MusicStoreContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                string hash = HashPassword(model.PasswordUser);
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordUser == hash);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.LoginUser),
                        new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Customer"),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.IdUser.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                   return RedirectToAction("Index", "Product");
                }
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.ErrorMessage = "Пользователь с таким логином существует";
                return View();
            }
            if (ModelState.IsValid)
            {
                model.PasswordUser = HashPassword(model.PasswordUser);
                model.RoleId = 1;

                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
