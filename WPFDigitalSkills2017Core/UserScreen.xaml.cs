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
            this.Close();
        }
    }
}