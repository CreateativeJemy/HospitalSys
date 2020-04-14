using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace HospitalSys.Domain.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public long FileNumber { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Nationality { get; set; }
        public MFemaleType MFemaleType { get; set; }
        public PatientType PatientType { get; set; }
        public string Image { get; set; }
        public IFormFile FileSource { get; set; }
        public Boolean PatientHistory { get; set; }
        public string Phone { get; set; }
        public string TelPhone { get; set; }
        public string DesisString { get; set; }
        public ICollection<PatientDesisesViewModel> PatientDesises { get; set; }
        public DateTime CreateDate { get; set; }

    }

}
