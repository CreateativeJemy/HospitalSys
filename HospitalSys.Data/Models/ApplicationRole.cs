using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text;

namespace HospitalSys.Data.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Access { get; set; }
    }
}
