using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Models
    {
        public Models()
        {
            Car = new HashSet<Car>();
        }

        public int IdModel { get; set; }
        public string ModelName { get; set; }
        public int IdMark { get; set; }

        public virtual Mark IdMarkNavigation { get; set; }
        public virtual ICollection<Car> Car { get; set; }
    }
}
