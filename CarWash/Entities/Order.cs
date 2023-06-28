using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Order
    {
        public Order()
        {
            ServicesInOrder = new HashSet<ServicesInOrder>();
        }

        public int IdOrder { get; set; }
        public int IdClient { get; set; }
        public int IdCar { get; set; }
        public int IdEmployee { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        public decimal TotalPriceOfOrder { get; set; }
        public int IdOrderStatus { get; set; }

        public virtual ClientsCars IdC { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual OrderStatus IdOrderStatusNavigation { get; set; }
        public virtual ICollection<ServicesInOrder> ServicesInOrder { get; set; }
    }
}
