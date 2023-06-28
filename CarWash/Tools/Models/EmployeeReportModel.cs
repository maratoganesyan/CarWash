using CarWash.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarWash.Tools.Models
{
    internal class EmployeeReportModel
    {
        public int Id;
        public string Employee;
        public List<OrderModel> Orders;
        public decimal TotalCostOfWorks;
        public DateTime StartDateOfReport;
        public DateTime EndDateOfReport;
        public EmployeeReportModel(Employee employee, List<Order> ordersOfEmployee,
            DateTime StartDate, DateTime EndDate)
        {
            Id = employee.IdEmployee;
            Employee = $"{employee.Surname} {employee.Name} {employee.Patronymic}";
            Orders = ordersOfEmployee.Select(o => new OrderModel(o)).ToList();
            TotalCostOfWorks = Orders.Select(o => o.TotalPriceOfOrder).Sum();
            StartDateOfReport = StartDate;
            EndDateOfReport = EndDate;  
        }
    }
}
