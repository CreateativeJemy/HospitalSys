using HospitalSys.Data.Context;
using HospitalSys.Data.DataAccess;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalSys.Domain.Services
{
    public class PatientDesisesService : Repository<PatientDesises>
    {
        private readonly Language language;
        public PatientDesisesService(ApplicationDbContext context, Language language)
          : base(context) { this.language = language; }

        public IEnumerable<PatientDesisesViewModel> Get()
        {
            return dbSet.Select(s => new PatientDesisesViewModel
            {
                Id = s.Id,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                DoctorName = s.DoctorName,
                EnterDate = s.EnterDate,
                OutDate = s.OutDate,
                Department = s.Department,
                Infected = s.Infected,
                Inhirtance = s.Inhirtance,
                Chronic = s.Chronic,
                Diagnose = s.Diagnose,
                PatientId = s.PatientId
            });
        }
        public PatientDesisesViewModel Get(int id)
        {
            return dbSet.Where(x => x.Id == id).Select(s => new PatientDesisesViewModel
            {
                Id = s.Id,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                DoctorName = s.DoctorName,
                EnterDate = s.EnterDate,
                OutDate = s.OutDate,
                Department = s.Department,
                Infected = s.Infected,
                Inhirtance = s.Inhirtance,
                Chronic = s.Chronic,
                Diagnose = s.Diagnose
            }).FirstOrDefault();
        }

        public PatientDesises Add(PatientDesisesViewModel s)
        {
            var newModel = new PatientDesises
            {
                Id = s.Id,
                PatientId = s.PatientId,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                DoctorName = s.DoctorName,
                EnterDate = s.EnterDate,
                OutDate = s.OutDate,
                Department = s.Department,
                Infected = s.Infected,
                Inhirtance = s.Inhirtance,
                Chronic = s.Chronic,
                Diagnose = s.Diagnose
            };
            Insert(newModel);
            return newModel;
        }

        public PatientDesises Edit(PatientDesisesViewModel s)
        {
            var newModel = new PatientDesises
            {
                Id = s.Id,
                PatientId = s.PatientId,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                DoctorName = s.DoctorName,
                EnterDate = s.EnterDate,
                OutDate = s.OutDate,
                Department = s.Department,
                Infected = s.Infected,
                Inhirtance = s.Inhirtance,
                Chronic = s.Chronic,
                Diagnose = s.Diagnose
            };
            Update(newModel);
            return newModel;
        }
        public void Delete(int id)
        {
            var model = dbSet.FirstOrDefault(w => w.Id == id);
            if (model != null)
                dbSet.Remove(model);
        }
    }
}
