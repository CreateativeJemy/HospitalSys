using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace HospitalSys.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PatientTypes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
