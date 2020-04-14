using HospitalSys.Data.Context;
using HospitalSys.Data.DataAccess;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalSys.Domain.Services
{
    public class PatientService : Repository<Patient>
    {
        private readonly Language language;
        public PatientService(ApplicationDbContext context, Language language)
           : base(context) { this.language = language; }

        public IEnumerable<PatientViewModel> Get()
        {
            return dbSet.Select(s => new PatientViewModel
            {
                Id = s.Id,
                FileNumber= s.FileNumber,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Nationality = s.Nationality,
                MFemaleType = (MFemaleType)s.MFemaleType,
                PatientType = (PatientType)s.PatientType,
                Phone = s.Phone,
                TelPhone = s.TelPhone,
                PatientHistory =s.PatientHistory,
                Image = s.Image,
                CreateDate = s.CreateDate
            });
        }
        public PatientViewModel Get(int id)
        {
            return dbSet.Where(x => x.Id == id).Include(z=> z.PatientDesises).Select(s => new PatientViewModel
            {
                Id = s.Id,
                FileNumber = s.FileNumber,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Nationality = s.Nationality,
                MFemaleType = (MFemaleType)s.MFemaleType,
                PatientType = (PatientType)s.PatientType,
                Phone = s.Phone,
                TelPhone = s.TelPhone,
                PatientHistory = s.PatientHistory,
                Image = s.Image,
                CreateDate = s.CreateDate,
                PatientDesises = s.PatientDesises.Select(x => new PatientDesisesViewModel
                {
                    Id = x.Id,
                    PatientId = x.PatientId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    DoctorName = x.DoctorName,
                    Department = x.Department,
                    Diagnose = x.Diagnose,
                    Chronic =x.Chronic,
                    Inhirtance =x.Inhirtance,
                    Infected =x.Infected,
                    EnterDate =x.EnterDate,
                    OutDate = x.OutDate
                }).ToList()
            }).FirstOrDefault();
        }
        public string GetMax()
        {
            if(dbSet.FirstOrDefault() == null)
            {
                return "1";
            }
            else
            {
                return (dbSet.Max(x => x.Id) + 1).ToString();
            }
        }

        public Patient Add(PatientViewModel s)
        {
            var newModel = new Patient
            {
                Id = s.Id,
                FileNumber = s.FileNumber,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Nationality = s.Nationality,
                MFemaleType = (int)s.MFemaleType,
                PatientType = (int)s.PatientType,
                Phone = s.Phone,
                TelPhone = s.TelPhone,
                PatientHistory = s.PatientHistory,
                Image = s.Image,
                CreateDate = DateTime.Now
            };
            Insert(newModel);
            return newModel;
        }

        public Patient Edit(PatientViewModel s)
        {
            var newModel = new Patient
            {
                Id = s.Id,
                FileNumber = s.FileNumber,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Nationality = s.Nationality,
                MFemaleType = (int)s.MFemaleType,
                PatientType = (int)s.PatientType,
                Phone = s.Phone,
                TelPhone = s.TelPhone,
                PatientHistory = s.PatientHistory,
                Image = s.Image,
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
