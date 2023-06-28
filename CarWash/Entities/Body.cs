using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Body
    {
        public Body()
        {
            Car = new HashSet<Car>();
        }

        public int IdBody { get; set; }
        public string BodyName { get; set; }

        public virtual ICollection<Car> Car { get; set; }
    }
}
