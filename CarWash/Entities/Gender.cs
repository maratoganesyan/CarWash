using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Gender
    {
        public Gender()
        {
            Clients = new HashSet<Clients>();
            Employee = new HashSet<Employee>();
        }

        public int IdGender { get; set; }
        public string GenderName { get; set; }

        public virtual ICollection<Clients> Clients { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
