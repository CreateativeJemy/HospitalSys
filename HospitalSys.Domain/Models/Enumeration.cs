using HospitalSys.Domain.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalSys.Domain.Models
{
    public enum Language
    {
        [Display(Name = "Arabic", ResourceType = typeof(Enumeration))]
        Arabic = 1,
        [Display(Name = "English", ResourceType = typeof(Enumeration))]
        English = 2
    }
    public enum MFemaleType
    {
        [Display(Name = "Male", ResourceType = typeof(Enumeration))]
        Male = 1,
        [Display(Name = "FeMale", ResourceType = typeof(Enumeration))]
        FeMale = 2
    }
    public enum PatientType
    {
        [Display(Name = "FreePatient", ResourceType = typeof(Enumeration))]
        FreePatient = 1,
        [Display(Name = "PaidPatient", ResourceType = typeof(Enumeration))]
        PaidPatient = 2,
        [Display(Name = "InsurancePatient", ResourceType = typeof(Enumeration))]
        InsurancePatient = 3,
        [Display(Name = "CountryPatient", ResourceType = typeof(Enumeration))]
        CountryPatient = 4,
    }
}
