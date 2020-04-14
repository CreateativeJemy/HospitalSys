using System;
using System.Linq;
using HospitalSys.Data.Context;
using HospitalSys.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DateTime = HospitalSys.CustomTimeZone.TimeZone;

namespace HospitalSys.Controllers
{
    public class LoggersController : BaseController
    {
        public LoggersController(ApplicationDbContext context,
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       RoleManager<ApplicationRole> roleManger,
       IHostingEnvironment env,
       IHttpContextAccessor _httpContextAccessor,
       DateTime dateTime) : base(context, userManager, signInManager, 
                roleManger, env, _httpContextAccessor, dateTime)
        {
            DateTime = dateTime;
        }

        public IActionResult Index()
        {
            try
            {
                var vms = UnitOfWork.LoggerService.Get().ToList();
                return View(vms);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}