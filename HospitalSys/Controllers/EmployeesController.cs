using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalSys.Data.Context;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HospitalSys.App_Code;
using DateTime = HospitalSys.CustomTimeZone.TimeZone;

namespace HospitalSys.Controllers
{
    public class EmployeesController : BaseController
    {
        public EmployeesController(ApplicationDbContext context,
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
        // employees actions
        public string GetTypes(string types)
        {
            string result = "";
            if (types != null)
            {
                int[] ia = types.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                foreach (var item in ia)
                {
                    result = result + ((PatientType)item).ToName().ToString() + " , ";
                }
                int index = result.LastIndexOf(',');
                return result.Remove(index, 1);
            }
            else
            {
                return result;
            }

        }
        public IActionResult CheckPatientTypesMultiS(string id)
        {
            var mod = UserManager.Users.FirstOrDefault(x => x.Id == id);
            string modSens = mod.PatientTypes;
            string s1 = modSens;
            if (s1 != null)
            {
                int[] ia = s1.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                return Json(new ResultViewModel { Data = ia });
            }
            else
            {
                return Json(new ResultViewModel { Data = null });
            }
        }
        public ActionResult Index()
        {
            var users = UserManager.Users.Select(x => new UserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.PhoneNumber,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PatientTypes = GetTypes(x.PatientTypes),
            }).ToList();
            return View(users);
        }
        public IActionResult UsersList()
        {
            var users = UserManager.Users.Select(x => new UserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.PhoneNumber,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PatientTypes = GetTypes(x.PatientTypes),
            }).ToList();
            return PartialView("_UserListView", users);
        }
        [HttpGet]
        public IActionResult UsersAdd()
        {
            var vm = new UserViewModel();
            return PartialView("_AddUserView", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UsersAdd(UserViewModel usrVM)
        {
            var message = string.Empty;
            var newUser = new ApplicationUser();
            try
            {
                ApplicationUser user = new ApplicationUser()
                {
                    PhoneNumber = usrVM.Phone,
                    UserName = usrVM.UserName,
                    Email = usrVM.Email,
                    PatientTypes = usrVM.PatientTypes,
                    FirstName = usrVM.FirstName,
                    LastName = usrVM.LastName
                };
                var res = await UserManager.CreateAsync(user, usrVM.Password);
                if (res.Succeeded)
                {
                    message = "Added";
                    newUser = user;
                }
            }
            catch (Exception ex)
            {
                var messs = ex;
            }
            return Json(new { message, newUser });
        }
        public IActionResult UsersEdit(string userId)
        {
            var usrVM = UserManager.Users.FirstOrDefault(x => x.Id == userId);
            var vm = new UserViewModel
            {
                Id = usrVM.Id,
                FirstName = usrVM.FirstName,
                LastName = usrVM.LastName,
                UserName = usrVM.UserName,
                Email = usrVM.Email,
                Phone = usrVM.PhoneNumber,
                PatientTypes = usrVM.PatientTypes,
                PatientType = usrVM.PatientTypes
            };
            return PartialView("_AddUserView", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UsersEdit(UserViewModel usrVM)
        {
            var message = string.Empty;
            var newUser = new ApplicationUser();
            try
            {
                var user = await UserManager.FindByIdAsync(usrVM.Id);
                if (user != null)
                {
                    user.FirstName = usrVM.FirstName;
                    user.LastName = usrVM.LastName;
                    user.Email = usrVM.Email;
                    user.UserName = usrVM.UserName;
                    user.PhoneNumber = usrVM.Phone;
                    user.PatientTypes = usrVM.PatientTypes;
                    var res = await UserManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        message = "Updated";
                        newUser.Id = usrVM.Id;
                        newUser = user;
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
            return Json(new { message, newUser });
        }
        public async Task<IActionResult> UserDetail(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }
        public async Task<IActionResult> UserDelete(string id)
        {
            bool res = false;
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                Microsoft.AspNetCore.Identity.IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    res = true;
                }
            }
            return Json(res);
        }
    }
}