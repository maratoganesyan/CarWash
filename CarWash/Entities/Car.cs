using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Car
    {
        public Car()
        {
            ClientsCars = new HashSet<ClientsCars>();
        }

        public int IdCar { get; set; }
        public int IdModel { get; set; }
        public string Description { get; set; }
        public string StateNumber { get; set; }
        public int IdBody { get; set; }
        public int IdColor { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual Body IdBodyNavigation { get; set; }
        public virtual Color IdColorNavigation { get; set; }
        public virtual Models IdModelNavigation { get; set; }
        public virtual ICollection<ClientsCars> ClientsCars { get; set; }
    }
}
