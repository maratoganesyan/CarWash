using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class AdditionalServices
    {
        public AdditionalServices()
        {
            ServicesInOrder = new HashSet<ServicesInOrder>();
        }

        public int IdService { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }

        public virtual ICollection<ServicesInOrder> ServicesInOrder { get; set; }
    }
}
