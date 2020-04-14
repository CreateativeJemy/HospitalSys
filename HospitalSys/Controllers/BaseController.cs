using System.Globalization;
using HospitalSys.Data.Context;
using HospitalSys.Data.Models;
using HospitalSys.Domain;
using HospitalSys.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DateTime = HospitalSys.CustomTimeZone.TimeZone;

namespace HospitalSys.Controllers
{
    public class BaseController : Controller
    {
        public readonly ApplicationDbContext context;
        public Language Language { get; set; }
        public IHostingEnvironment env;
        public UnitOfWork UnitOfWork { get; set; }
        public UserManager<ApplicationUser> UserManager;
        public readonly RoleManager<ApplicationRole> RoleManger;
        public SignInManager<ApplicationUser> SignInManager;
        public readonly IHttpContextAccessor httpContextAccessor;
        public DateTime DateTime { get; set; }
        public BaseController(ApplicationDbContext _context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManger, IHostingEnvironment _env,
            IHttpContextAccessor _httpContextAccessor, DateTime _DateTime)
        {
            UserManager = userManager;
            RoleManger = roleManger;
            SignInManager = signInManager;
            context = _context;
            Language = Language.Arabic;
            ViewBag.Language = "en";
            UnitOfWork = new UnitOfWork(context,Language);
            env = _env;
            httpContextAccessor = _httpContextAccessor;
            DateTime = _DateTime;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
            var languageCookie = filterContext.HttpContext.Request.Cookies["Language"];

            if (filterContext.HttpContext.Request.Cookies["CustomTimeZone"] != null)
            {
                DateTime.MyTimeZone = filterContext.HttpContext.Request.Cookies["CustomTimeZone"];
            }
            else
            {
                DateTime.MyTimeZone = "Africa/Cairo";
                //DateTime.MyTimeZone = "America/New_York";
            }
            if (languageCookie != null)
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(languageCookie);
                ViewBag.Language = languageCookie;
                switch (languageCookie)
                {
                    case "ar":
                        ViewBag.DataTableTranslation = "datatables.Arabic.json";
                        ViewBag.Language = "ar";
                        Language = Language.Arabic;
                        UnitOfWork = new UnitOfWork(context, Language);
                        break;
                    case "en":
                        ViewBag.DataTableTranslation = "datatables.english.json";
                        ViewBag.Language = "en";
                        Language = Language.English;
                        UnitOfWork = new UnitOfWork(context, Language);
                        break;
                }
            }
            else
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("ar");
                ViewBag.DataTableTranslation = "datatables.Arabic.json";
                ViewBag.Language = "ar";
                Language = Language.Arabic;
                UnitOfWork = new UnitOfWork(context, Language);
            }
        }
    }
}