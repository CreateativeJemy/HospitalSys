using HospitalSys.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace HospitalSys.Domain.Models
{
    public class LoginVM
    {
        [Display(Name = "UserName", ResourceType = typeof(Enumeration))]
        public string UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessageResourceName = "PleaseenterYourEmail", ErrorMessageResourceType = typeof(Enumeration))]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(Enumeration))]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }

}
