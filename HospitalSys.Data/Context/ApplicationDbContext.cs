using HospitalSys.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSys.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientDesises> PatientDesises { get; set; }
        public DbSet<Logger> Loggers { get; set; }
    }
}
