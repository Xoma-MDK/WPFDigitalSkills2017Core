using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDigitalSkills2017Core.Models
{
    internal class UserDG
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public bool? Active { get; set; }
        public virtual Office? Office { get; set; }
        public string Role { get; set; } = null!;
    }
}
