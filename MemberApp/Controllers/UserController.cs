using MemberApp.Models;
using MemberApp.Controllers;
using MemberApp.Data;
using MemberApp.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemberApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        #region Register

        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            // 檢查帳號是否已存在
            var existing = await _context.Users
                                         .AnyAsync(u => u.Account == dto.Account);
            if (existing)
            {
                ModelState.AddModelError(string.Empty,
                                         "此帳號已被註冊，請換一個帳號。");
                return View(dto);
            }

            // 建立 UserModel 並設定密碼
            var user = new UserModel
            {
                Account = dto.Account
            };
            user.SetPassword(dto.Password);

            // 存入資料庫
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 自動登入（可選）並跳轉
            await SignInUserAsync(user);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Login

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(dto);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Account == dto.Account);

            if (user == null || !user.VerifyPassword(dto.Password))
            {
                ModelState.AddModelError(string.Empty,
                                         "帳號或密碼錯誤，請重新輸入。");
                return View(dto);
            }

            await SignInUserAsync(user);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Helpers

        private async Task SignInUserAsync(UserModel user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Account)
            };
            var identity = new ClaimsIdentity(claims,
                                               CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          principal,
                                          new AuthenticationProperties
                                          {
                                              IsPersistent = true,
                                              ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                                          });
        }

        #endregion
    }
}