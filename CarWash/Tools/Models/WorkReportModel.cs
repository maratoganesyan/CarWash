using CarWash.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Tools.Models
{
    internal class WorkReportModel
    {
        public List<OrderModel> Orders;
        public decimal TotalCostOfReport;
        public DateTime StartDateOfReport;
        public DateTime EndDateOfReport;
        public WorkReportModel(List<OrderModel> Orders,
            decimal TotalCostOfReport,
            DateTime StartDateOfReport,
            DateTime EndDateOfReport)
        {
            this.Orders = Orders;
            this.TotalCostOfReport = TotalCostOfReport;
            this.StartDateOfReport = StartDateOfReport;
            this.EndDateOfReport = EndDateOfReport;
        }
    }
}
