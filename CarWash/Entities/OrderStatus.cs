using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Order = new HashSet<Order>();
        }

        public int IdOrderStatus { get; set; }
        public string OrderStatusName { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
