using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Clients
    {
        public Clients()
        {
            ClientsCars = new HashSet<ClientsCars>();
        }

        public int IdClient { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public int IdGender { get; set; }

        public virtual Gender IdGenderNavigation { get; set; }
        public virtual ICollection<ClientsCars> ClientsCars { get; set; }
    }
}
