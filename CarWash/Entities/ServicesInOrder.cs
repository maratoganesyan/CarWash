using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class ServicesInOrder
    {
        public int IdOrder { get; set; }
        public int IdService { get; set; }

        public virtual Order IdOrderNavigation { get; set; }
        public virtual AdditionalServices IdServiceNavigation { get; set; }
    }
}
