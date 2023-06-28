using System;
using System.Collections.Generic;
using System.Linq;
using CarWash.Entities;

namespace CarWash.Tools.Models
{
    internal class OrderModel
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public string ClientPhone { get; set; }
        public string Car { get; set; }
        public string Employee { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        public decimal TotalPriceOfOrder { get; set; }
        public List<AdditionalServices> Services { get; set; }
        public OrderModel(Order order)
        {
            Id = order.IdOrder;
            Client = $"{order.IdC.IdClientNavigation.Surname} " +
                $"{order.IdC.IdClientNavigation.Name.ElementAt(0)}. " +
                $"{order.IdC.IdClientNavigation.Patronymic.ElementAt(0)}.";
            ClientPhone = order.IdC.IdClientNavigation.PhoneNumber;
            Car = $"{order.IdC.IdCarNavigation.IdModelNavigation.IdMarkNavigation.MarkName} " +
                $"{order.IdC.IdCarNavigation.IdModelNavigation.ModelName} " +
                $"{order.IdC.IdCarNavigation.IdColorNavigation.ColorName}";
            Employee = $"{order.IdEmployeeNavigation.Surname} " +
                $"{order.IdEmployeeNavigation.Name.ElementAt(0)}. " +
                $"{order.IdEmployeeNavigation.Patronymic.ElementAt(0)}.";
            DateTimeOfOrder = order.DateTimeOfOrder;
            TotalPriceOfOrder = order.TotalPriceOfOrder;
            Services = order.ServicesInOrder.Select(service => service.IdServiceNavigation).ToList();
        }
        public override string ToString()
        {
            return $"{Client} | {Car} | {string.Join(", ", Services.Select(s => s.ServiceName))}";
        }
    }
}
