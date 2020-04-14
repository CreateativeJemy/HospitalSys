using HospitalSys.Data.Context;
using HospitalSys.Domain.Models;
using HospitalSys.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSys.Domain
{
    public class UnitOfWork
    {
        private bool _disposed;
        public Language language { get; set; }
        public ApplicationDbContext Context { get; private set; }
        public UnitOfWork(ApplicationDbContext context, Language Lang)
        {
            Context = context;
            language = Lang;
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                var sr = ex.Message;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && Context != null)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region PatientService
        PatientService patientService;
        public PatientService PatientService => patientService ?? (patientService = new PatientService(Context , language));
        #endregion
        #region Messages
        PatientDesisesService patientDesisesService;
        public PatientDesisesService PatientDesisesService => patientDesisesService ?? (patientDesisesService = new PatientDesisesService(Context , language));
        #endregion

        #region Messages
        LoggerService loggerService;
        public LoggerService LoggerService => loggerService ?? (loggerService = new LoggerService(Context, language));
        #endregion
    }

}
