using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class ClientsCars
    {
        public ClientsCars()
        {
            Order = new HashSet<Order>();
        }

        public int IdClient { get; set; }
        public int IdCar { get; set; }

        public virtual Car IdCarNavigation { get; set; }
        public virtual Clients IdClientNavigation { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
