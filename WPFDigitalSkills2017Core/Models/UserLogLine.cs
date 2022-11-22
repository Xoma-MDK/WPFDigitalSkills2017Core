using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDigitalSkills2017Core.Models
{
    internal class UserLogLine
    {
        public DateOnly Date { get; set; }
        public TimeOnly LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string TimeSpentOnSystem { get; set; }
        public bool Crash { get; set; }
        public string Reason { get; set; }
    }
}
