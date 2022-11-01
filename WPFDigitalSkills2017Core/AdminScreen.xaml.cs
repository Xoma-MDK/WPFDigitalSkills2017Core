using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using Microsoft.EntityFrameworkCore;
using WPFDigitalSkills2017Core.Models;

namespace WPFDigitalSkills2017Core;

/// <summary>
/// Логика взаимодействия для AdminScreen.xaml
/// </summary>
public partial class AdminScreen : Window
{
    private User _user;
    private Activityofuse _activityofuse;
    public AdminScreen(User userInput, Activityofuse activityofuse)
    {
        _user = userInput;
        _activityofuse = activityofuse;
        InitializeComponent();
        using (var bd = new Session106Context())
        {
            bd.Offices.Load();
            var offices = bd.Offices.Local.ToBindingList();
            foreach (var office in offices)
            {
                CbOffice.Items.Add(office.Title);
            }
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

    private void MiAddUser_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CbOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DgUsersInfo.Items.Clear();
        if (CbOffice.SelectedIndex == 0)
        {
            using (var bd = new Session106Context())
            {
                bd.Offices.Load();
                bd.Users.Load();
                bd.Roles.Load();
                var users = bd.Users.Local.ToBindingList();
                foreach (var user in users)
                {
                    DgUsersInfo.Items.Add(user);
                }
            }
        }
        else
        {
            using (var bd = new Session106Context())
            {
                var office = bd.Offices.Where(o => o.Title == CbOffice.SelectedItem.ToString()).ToList()[0].Id;
                bd.Users.Where(u => u.OfficeId == office).Load();
                bd.Roles.Load();
                var users = bd.Users.Local.ToBindingList();
                foreach (var user in users)
                {
                    DgUsersInfo.Items.Add(user);
                }
            }
        }

    }
}