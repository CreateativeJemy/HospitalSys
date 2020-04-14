using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HospitalSys.App_Code;
using HospitalSys.Data.Context;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DateTime = HospitalSys.CustomTimeZone.TimeZone;

namespace HospitalSys.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext context,
         UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         RoleManager<ApplicationRole> roleManger,
         IHostingEnvironment env, 
         IHttpContextAccessor _httpContextAccessor,
         DateTime dateTime) 
            : base(context, userManager, signInManager, 
                  roleManger, env, _httpContextAccessor, dateTime)
        {
            DateTime = dateTime;
        }
        public void Log(string userId, string action, string localIp, Boolean loginStatus)
        {
            var model = new LoggerViewModel { Action = action , LocalIp= localIp , LoginStatus = loginStatus , Date = DateTime.Now, UserId = userId };
            var date = model.Date;
            UnitOfWork.LoggerService.Add(model);
            UnitOfWork.Commit();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
            public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated && User.IsInRole("Administrator"))
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("/Patient/Index");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                return View(new LoginVM
                {
                    ReturnUrl = returnUrl
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            HttpContext.Session.SetString("CustomTimeZone", "Africa/Cairo");
            var user = await UserManager.FindByEmailAsync(loginViewModel.Email);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Language == Language.Arabic ? "البريد الاليكترونى غير موجود" : "Email not found");
                return View(loginViewModel);
            }
            else if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("MySuberSecureKey");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.Now.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var token_val = new JwtSecurityTokenHandler().WriteToken(token);
                    Log(user.Id, "دخول", GetLocalIPAddress(), true);
                    return Redirect("/Patient/Index");
                }
                else
                {
                    ModelState.AddModelError("", Language == Language.Arabic ? " كلمة المرور غير صحيحة" : "Password not Correct");
                    Log(user.Id, "دخول", GetLocalIPAddress(), false);
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("", Language == Language.Arabic ? "البريد الاليكترونى غير موجود" : "Email not found");
                Log(user.Id, "دخول", GetLocalIPAddress(), false);
                return View(loginViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userid = User.GetUserId();
            await SignInManager.SignOutAsync();
            Log(userid,"خروج", GetLocalIPAddress(), true);
            return Redirect("/Account/Login");
        }
    }
}