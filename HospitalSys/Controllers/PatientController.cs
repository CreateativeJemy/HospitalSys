using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HospitalSys.App_Code;
using HospitalSys.Data.Context;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DateTime = HospitalSys.CustomTimeZone.TimeZone;

namespace HospitalSys.Controllers
{
    public class PatientController : BaseController
    {
        public PatientController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManger,
        IHostingEnvironment env,
        IHttpContextAccessor _httpContextAccessor,
        DateTime dateTime) : base(context, userManager, signInManager,
            roleManger, env, _httpContextAccessor,dateTime)
        {
            DateTime = dateTime;
        }
        #region Patients
        public ActionResult Joins()
        {
            #region inner join
            var result = UnitOfWork.PatientService.Get()
                        .Join
                        (UnitOfWork.PatientDesisesService.Get(),
                            e => e.Id,
                            d => d.PatientId,
                            (patient, patientDis) => new
                            {
                                Name = patient.NameAr,
                                DesisesName = patientDis.NameAr
                            }
                       );
            foreach (var item in result)
            {
                string res = item.Name + item.DesisesName;
            }
            #endregion
            #region left join
            /*
              To implement Left Outer Join, with extension method syntax
              we use the GroupJoin() method along with SelectMany() and DefaultIfEmpty() methods.
            */
            var result2 =
                          UnitOfWork.PatientService.Get()
                         .GroupJoin
                         (
                          UnitOfWork.PatientDesisesService.Get(),
                          e => e.Id,
                          d => d.PatientId,
                          (pat, dis) => new { pat, dis }
                         )
                         // left
                         .SelectMany(z => z.dis.DefaultIfEmpty(),
                        // right
                        // .SelectMany(z => z.dis.DefaultIfEmpty(),
                        (a, b) => new
                        {
                            Name = a.pat.NameAr,
                            DisName = b == null ? "No Des" : b.NameAr
                        });

            foreach (var v in result2)
            {
                string res = v.Name + "\t" + v.DisName;
            }
            #endregion
            #region cross join
            // To implement Cross Join using extension method syntax, we could either use SelectMany() method or Join() method
            // Implementing cross join using SelectMany()
            var result3 = UnitOfWork.PatientService.Get()
            .SelectMany(e => UnitOfWork.PatientDesisesService.Get(), (e, d) => new { e, d });

            foreach (var v in result3)
            {
                Console.WriteLine(v.e.NameAr + "\t" + v.d.NameAr);
            }

            // Implementing cross join using Join() Return all records where each 
            // row from the first table is combined with each row from the second table.
            var result4 = UnitOfWork.PatientService.Get()
            .Join(UnitOfWork.PatientDesisesService.Get(),
            e => true,
            d => true,
            (e, d) => new { e, d });

            foreach (var v in result4)
            {
                Console.WriteLine(v.e.NameAr + "\t" + v.d.NameAr);
            }
            #endregion
            return View();
        }
        public IActionResult Index()
        {
            ViewBag.TimeNow = DateTime.Now;
            var model = UnitOfWork.PatientService.Get().ToList();
            return View(model);
        }
        public IActionResult PatientsList()
        {
            var model = UnitOfWork.PatientService.Get().ToList();
            return PartialView("_PatientListView", model);
        }
        [HttpGet]
        public IActionResult PatientsAdd()
        {
            var vm = new PatientViewModel();
            return PartialView("_PatientAddView", vm);
        }
        [HttpPost]
        public IActionResult PatientsAdd(PatientViewModel model)
        {
            var disis = JsonConvert.DeserializeObject<List<PatientDesisesViewModel>>(model.DesisString);
            string path = Path.Combine(env.WebRootPath, "Files/PatientFiles");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileUrl = model.FileSource;
            if (fileUrl != null && fileUrl.Length > 0)
            {
                var name = Guid.NewGuid().ToString() + fileUrl.FileName;
                using (var ms = new FileStream(Path.Combine(path, name), FileMode.Create))
                {
                    fileUrl.CopyTo(ms);
                }
                model.Image = $"Files/PatientFiles/" + name;
            }
            string maxFileNum = UnitOfWork.PatientService.GetMax();
            int lengthMaxFileNum = 4 - maxFileNum.Length;
            for(int i = 0; i < lengthMaxFileNum;i++)
            {
                maxFileNum = "0" + maxFileNum;
            }
            string fileNumber =DateTime.Now.ToString("yyyyMMdd") + maxFileNum;
            model.FileNumber = Convert.ToInt64(fileNumber);
            var modelDb = UnitOfWork.PatientService.Add(model);
            UnitOfWork.Commit();
            if (disis.Count() > 0)
            {
                foreach (var item in disis)
                {
                    item.PatientId = modelDb.Id;
                    UnitOfWork.PatientDesisesService.Add(item);
                    UnitOfWork.Commit();
                }
            }
            return Json(new { IsSuccess = true });
        }
        [HttpGet]
        public async Task<IActionResult> PatientsEdit(int id)
        {
            var user = UserManager.Users.FirstOrDefault(x => x.Id == User.GetUserId());
            var patient = UnitOfWork.PatientService.Get(id);
            if(await UserManager.IsInRoleAsync(user, "Administrator") || user.PatientTypes.Contains(((int)patient.PatientType).ToString()))
            {
                return PartialView("_PatientAddView", patient);
            }
            else
            {
                return Json(new ResultViewModel { IsSuccess = false,Message ="لا تملك حق التعديل"});
            }
        }
        [HttpPost]
        public IActionResult PatientsEdit(PatientViewModel model)
        {
            var oldmodel = UnitOfWork.PatientService.Get(model.Id);
            var disis = JsonConvert.DeserializeObject<List<PatientDesisesViewModel>>(model.DesisString);
            string path = Path.Combine(env.WebRootPath, "Files/PatientFiles");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fileUrl = model.FileSource;
            if (fileUrl != null && fileUrl.Length > 0)
            {
                var oldPlaceImg = oldmodel.Image;
                if (oldPlaceImg != null)
                {
                    int indx = oldPlaceImg.LastIndexOf("/");
                    oldPlaceImg = oldPlaceImg.Substring(indx);
                    if (System.IO.File.Exists(path + oldPlaceImg))
                    {
                        System.IO.File.Delete(path + oldPlaceImg);
                    }
                }
                var name = Guid.NewGuid().ToString() + fileUrl.FileName;
                using (var ms = new FileStream(Path.Combine(path, name), FileMode.Create))
                {
                    fileUrl.CopyTo(ms);
                }
                model.Image = $"Files/PatientFiles/" + name;
            }
            else
            {
                var mod = UnitOfWork.PatientService.Get(model.Id);
                model.Image = mod.Image;
            }
            UnitOfWork.PatientService.Edit(model);
            UnitOfWork.Commit();
            if (disis.Count() > 0)
            {
                foreach (var item in disis)
                {
                    item.PatientId = oldmodel.Id;
                    UnitOfWork.PatientDesisesService.Add(item);
                    UnitOfWork.Commit();
                }
            }

            return Json(new { IsSuccess = true });
        }
        [HttpPost]
        public IActionResult DeletePatient(int id)
        {
            UnitOfWork.PatientService.Delete(id);
            UnitOfWork.Commit();
            return Json(new { IsSuccess = true, Message = "تم الحذف بنجاح" });
        }
        public IActionResult DeleteDises(int id)
        {
            UnitOfWork.PatientDesisesService.Delete(id);
            UnitOfWork.Commit();
            return Json(new { IsSuccess = true, Message = "تم الحذف بنجاح" });
        }
        [HttpGet]
        public IActionResult PatientsDetail(int id)
        {
            var vm = UnitOfWork.PatientService.Get(id);
            return PartialView("_DetailPatientView", vm);
        }
        #endregion
    }
}