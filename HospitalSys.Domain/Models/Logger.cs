using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalSys.Domain.Models
{
    public class LoggerViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string LocalIp { get; set; }
        public Boolean LoginStatus { get; set; }
    }
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "ادخل اسم الموظف")]
        [Display(Name = "اسم الموظف")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ادخل البريد الاليكترونى")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientTypes { get; set; }
        public string PatientType { get; set; }
        public string Phone { get; set; }
    }
}
