using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CarWash.Entities;
using CarWash.Tools.Models;

namespace CarWash.Tools
{
    internal static class DbUtils
    {
        public readonly static Db db;

        static DbUtils()
        {
            try
            {
                db = new Db();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подлкючения к базе данных. Проверьте подключение!");
                Application.Current.Shutdown();
            }
        }
        public static List<OrderModel> GetOrderModelsByDates(DateTime StartDate, DateTime EndDate, out decimal TotalCostOfOrders)
        {
            List<OrderModel> Orders = db
                .Order
                .Where(o => StartDate <= o.DateTimeOfOrder &&
                    o.DateTimeOfOrder <= EndDate &&
                    o.IdOrderStatusNavigation.OrderStatusName == "Выполнен")
                .Select(o => new OrderModel(o))
                .ToList();
            TotalCostOfOrders = Orders.Select(o => o.TotalPriceOfOrder).Sum();
            return Orders;
        }
        public static (DateTime Earlier, DateTime Lastest) GetTheDeadlineDates(Employee employee)
        {
            if (employee.Order.Count > 0)
            {
                return (employee.Order.Select(o => o.DateTimeOfOrder).Min(),
                employee.Order.Select(o => o.DateTimeOfOrder).Max());
            }
            else
            {
                return (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59));
            }
        }
        public static List<Order> GetEmployeeOrderByDate(Employee employee, DateTime StartDate, DateTime EndDate)
            => employee.Order.Where(o => StartDate <= o.DateTimeOfOrder && o.DateTimeOfOrder <= EndDate).ToList();

    }
}
