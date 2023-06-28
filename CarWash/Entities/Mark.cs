using System;
using System.Collections.Generic;

namespace CarWash.Entities
{
    public partial class Mark
    {
        public Mark()
        {
            Models = new HashSet<Models>();
        }

        public int IdMark { get; set; }
        public string MarkName { get; set; }

        public virtual ICollection<Models> Models { get; set; }
    }
}
