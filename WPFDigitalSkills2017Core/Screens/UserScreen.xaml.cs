using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFDigitalSkills2017Core.Models;

namespace WPFDigitalSkills2017Core;

/// <summary>
/// Логика взаимодействия для UserScreen.xaml
/// </summary>
public partial class UserScreen : Window
{
    private User _user;
    private Activityofuse _activityofuse;
    public UserScreen(User user, Activityofuse activityofuse)
    {
        _user = user;
        _activityofuse = activityofuse;
        InitializeComponent();
        LbWelcome.Content = $"Hi {_user.FirstName}, Welcome to AMONIC Airlines Automation System";
        using (var bd = new Session106Context())
        {
            var expiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
            var activityofuselist = bd.Activityofuses.Where(o => o.UserId == _user.Id).OrderBy(o => o.Date).Where(o => o.Date > expiryDate).ToList();
            TimeSpan time = TimeSpan.Zero;
            var crashs = 0;
            List<UserLogLine> logs = new List<UserLogLine>();
            foreach (var a in activityofuselist)
            {
                if (a == activityofuselist.Last()) 
                    break;
                if (a.LogoutTime > a.LoginTime)
                    //time.Add(a.LogoutTime - a.LoginTime);
                    time += a.LogoutTime - a.LoginTime;
                if (a.Reason != String.Empty)
                    crashs++;
                logs.Add(new UserLogLine
                {
                    Crash = a.Reason != String.Empty? true: false,
                    LoginTime = TimeOnly.FromDateTime(a.LoginTime),
                    Date = a.Date,
                    TimeSpentOnSystem = a.LogoutTime > a.LoginTime ? a.TimeSpentOnSystem.ToString(): "**",
                    LogoutTime = a.LogoutTime > a.LoginTime? TimeOnly.FromDateTime(a.LogoutTime).ToString() : "**",
                    Reason = a.Reason,
                });
            }

            foreach (var log in logs)
            {
                DgUserLogInfo.Items.Add(log);
            }
            LbTimeSpent.Content = $"Time spent on system: {time}";
            LbNumberOfCrashes.Content = $"Number of crashes: {crashs}";
        }
    }

    private void MiExit_Click(object sender, RoutedEventArgs e)
    {
        using (var bd = new Session106Context())
        {
            _activityofuse.LogoutTime = DateTime.Now;
            var subTime = _activityofuse.LogoutTime.Subtract(_activityofuse.LoginTime);
            var timeInSystem = TimeOnly.FromTimeSpan(subTime);
            _activityofuse.TimeSpentOnSystem = timeInSystem;
            bd.Activityofuses.Update(_activityofuse);
            bd.SaveChanges();
            new MainWindow().Show();
            Close();
        }
    }
}