using HospitalSys.Data.Context;
using HospitalSys.Data.DataAccess;
using HospitalSys.Data.Models;
using HospitalSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalSys.Domain.Services
{
    public class LoggerService : Repository<Logger>
    {
        private readonly Language language;
        public LoggerService(ApplicationDbContext context, Language language)
           : base(context) { this.language = language; }

        public IEnumerable<LoggerViewModel> Get()
        {
            return dbSet.Select(s => new LoggerViewModel
            {
                Id = s.Id,
                Action = s.Action,
                Date = s.Date,
                LocalIp = s.LocalIp,
                LoginStatus = s.LoginStatus,
                UserName = s.User.UserName,
                UserId = s.UserId,
            });
        }
        public LoggerViewModel Get(int id)
        {
            return dbSet.Where(x => x.Id == id).Select(s => new LoggerViewModel
            {
                Id = s.Id,
                Action = s.Action,
                Date = s.Date,
                LocalIp = s.LocalIp,
                UserName = s.User.UserName,
                LoginStatus = s.LoginStatus,
                UserId = s.UserId,
            }).FirstOrDefault();
        }

        public Logger Add(LoggerViewModel s)
        {
            var newModel = new Logger
            {
                Id = s.Id,
                Action = s.Action,
                Date = s.Date,
                LocalIp = s.LocalIp,
                LoginStatus = s.LoginStatus,
                UserId = s.UserId,
            };
            Insert(newModel);
            return newModel;
        }

        public Logger Edit(LoggerViewModel s)
        {
            var newModel = new Logger
            {
                Id = s.Id,
                Action = s.Action,
                Date = s.Date,
                LocalIp = s.LocalIp,
                LoginStatus = s.LoginStatus,
                UserId = s.UserId,
            }; Update(newModel);
            return newModel;
        }
        public void Delete(int id)
        {
            var model = dbSet.FirstOrDefault(w => w.Id == id);
            if (model != null)
            dbSet.Remove(model);
        }
    }
}
