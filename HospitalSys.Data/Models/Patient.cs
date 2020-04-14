using System;
using System.Collections.Generic;

namespace HospitalSys.Data.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public long FileNumber { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Nationality { get; set; }
        public int MFemaleType { get; set; }
        public int PatientType { get; set; }
        public string Image { get; set; }
        public Boolean PatientHistory { get; set; }
        public string Phone { get; set; }
        public string TelPhone { get; set; }
        public ICollection<PatientDesises> PatientDesises { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
