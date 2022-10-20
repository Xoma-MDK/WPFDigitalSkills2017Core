using System;
using System.Collections.Generic;

namespace WPFDigitalSkills2017Core.Models
{
    public partial class Country
    {
        public Country()
        {
            Offices = new HashSet<Office>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Office> Offices { get; set; }
    }
}
