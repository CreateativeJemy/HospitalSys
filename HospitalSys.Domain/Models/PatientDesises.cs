using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSys.Domain.Models
{
    public class PatientDesisesViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public Boolean Chronic { get; set; }
        public Boolean Inhirtance { get; set; }
        public Boolean Infected { get; set; }
        public string DoctorName { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime OutDate { get; set; }
        public string Diagnose { get; set; }
        public string Department { get; set; }
    }
}
