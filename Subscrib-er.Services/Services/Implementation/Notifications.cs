using Subscrib_er.Entities;
using Subscrib_er.Services.Interface;
using Subscrib_er.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subscrib_er.Services.Services.Implementation
{
    public class Notifications : INotifications
    {
        private readonly IRepository<Package> package;
        private readonly IRepository<Payments> payment;

        public Notifications(IRepository<Package> package, IRepository<Payments> payment)
        {
            this.package = package;
            this.payment = payment;
        }

        public double Active(string id)
        {
            return package.GetAll().Where(s => s.userId == id && s.Status == packagestate.Active).Count();
        }

        public double All(string id)
        {
            return package.GetAll().Where(s => s.userId == id).Count();
        }

        public int DueDate(string id)
        {
            //get the d
            DateTime today = DateTime.Today;
            return payment.GetAll().Where(x => x.userId == id && x.endDate.Subtract(today).TotalDays == 2).Count();
        }

        public double Expired(string id)
        {
            return package.GetAll().Where(u => u.userId == id && u.Status == packagestate.Expired).Count();            
        }

        public double Inactive(string id)
        {
            return package.GetAll().Where(s => s.userId == id && s.Status == packagestate.Inactive).Count();
        }

        public double ProgressBar(Guid id)
        {
            DateTime today = DateTime.Today;
            //get the start date
            DateTime startday = payment.GetAll().Where(x => x.packageId == id).Select(d => d.startDate).FirstOrDefault();

            //get the number of days between the start date and the current date
            double daysCount = Math.Abs(today.Day - startday.Day);

            //get the end date
            DateTime endday = payment.GetAll().Where(x => x.packageId == id).Select(d => d.endDate).FirstOrDefault();

            //get the total number of days
            double totaldays = Math.Abs(endday.Day - startday.Day);

            return Math.Round((daysCount/totaldays)*100, 2);
        }

        public double TotalCost(string id)
        {
            return payment.GetAll().Where(s => s.userId == id).Sum(x => x.amount);
        }

        public double ActiveBar(string id)
        {
            double active = Active(id);
            double all = All(id);
            return Math.Round((active / all) * 100, 2);
        }

        public double InactiveBar(string id)
        {
            double inactive = Inactive(id);
            double all = All(id);
            return Math.Round((inactive / all) * 100, 2);
        }

        public double ExpiredBar(string id)
        {
            double exp = Expired(id);
            double all = All(id);
            return Math.Round((exp / all) * 100, 2);
        }

        public List<Payments> listExpired(string id)
        {
            DateTime today = DateTime.Today;
            //get the payment object
            return payment.GetAll().Where(x => x.userId == id && x.endDate.Subtract(today).TotalDays == 2).ToList();

        }
        public double PackageActiveBar(string id, Guid packId)
        {
            double active = package.GetAll().Where(x => x.userId == id && x.Id == packId && x.Status == packagestate.Active).Count();
            double all = All(id);
            return Math.Round((active / all) * 100, 2);
        }
    }
}
