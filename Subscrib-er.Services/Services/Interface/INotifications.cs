using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscrib_er.Services.Services.Interface
{
    public interface INotifications
    {
        public int DueDate(string id);
        public List<Payments> listExpired(string id);
        public double ProgressBar(Guid id);

        public double Expired(string id);
        public double Active(string id);
        public double All(string id);
        public double Inactive(string id);
        public double TotalCost(string id);
        public double PackageActiveBar(string id, Guid packId);
        public double ActiveBar(string id);
        public double InactiveBar(string id);
        public double ExpiredBar(string id);
    }
}
