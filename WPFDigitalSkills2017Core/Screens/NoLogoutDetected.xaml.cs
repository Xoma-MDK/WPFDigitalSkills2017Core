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
/// Логика взаимодействия для NoLogoutDetected.xaml
/// </summary>
public partial class NoLogoutDetected : Window
{
    private User _user;
    private Activityofuse _activityofuse;
    public NoLogoutDetected(User user, Activityofuse activityofuse )
    {
        InitializeComponent();
        LbInfo.Content += $"{activityofuse.LoginTime.Day}/{activityofuse.LoginTime.Month}/{activityofuse.LoginTime.Year} at {activityofuse.LoginTime.Hour}:{activityofuse.LoginTime.Minute}";
        _user = user;
        _activityofuse = activityofuse;
    }

    private void BtnConfirm_Click(object sender, RoutedEventArgs e)
    {
        if (TbReason.Text == "")
        {
            MessageBox.Show("Reason empty", "Error");
            return;
        }

        if (RbntSoftwareCrash.IsChecked == false && RbntSystemCrash.IsChecked == false)
        {
            MessageBox.Show("Type crash not set", "Error");
            return;
        }

        if (RbntSoftwareCrash.IsChecked == true) _activityofuse.Reason = "Software Crash\n";
        if (RbntSystemCrash.IsChecked == true) _activityofuse.Reason = "System Crash\n";
        _activityofuse.Reason += TbReason.Text;
        using (var bd = new Session106Context())
        {
            bd.Activityofuses.Update(_activityofuse);
            var activityOfUseNote = new Activityofuse
            {
                UserId = _user.Id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                LoginTime = DateTime.Now,
                Reason = ""
            };
            bd.Activityofuses.Add(activityOfUseNote);
            bd.SaveChanges();
            if (_user.RoleId == 1)
            {
                new AdminScreen(_user, activityOfUseNote).Show();
                Close();
            }
            else if (_user.RoleId == 2)
            {
                new UserScreen(_user, activityOfUseNote).Show();
                Close();
            }
        }

            

    }
}