using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSys.Data.Models
{
    public class Logger
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string LocalIp  { get; set; }
        public Boolean LoginStatus  { get; set; }
    }
}
