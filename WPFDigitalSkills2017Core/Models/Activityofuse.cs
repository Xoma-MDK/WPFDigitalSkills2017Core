using System;
using System.Collections.Generic;

namespace WPFDigitalSkills2017Core.Models
{
    public partial class Activityofuse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public TimeOnly TimeSpentOnSystem { get; set; }
        public string Reason { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
