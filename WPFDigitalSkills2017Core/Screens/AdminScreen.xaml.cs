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
        using (var db = new Session106Context())
        {
            db.Offices.Load();
            var offices = db.Offices.Local.ToBindingList();
            foreach (var office in offices)
            {
                CbOffice.Items.Add(office.Title);
            }
        }
            
    }

    private void MiExit_Click(object sender, RoutedEventArgs e)
    {
        using (var db = new Session106Context())
        {
            _activityofuse.LogoutTime = DateTime.Now;
            var subTime = _activityofuse.LogoutTime.Subtract(_activityofuse.LoginTime);
            var timeInSystem = TimeOnly.FromTimeSpan(subTime);
            _activityofuse.TimeSpentOnSystem = timeInSystem;
            db.Activityofuses.Update(_activityofuse);
            db.SaveChanges();
            new MainWindow().Show();
            Close();
        }
    }

    private void MiAddUser_Click(object sender, RoutedEventArgs e)
    {
        new AddUser().ShowDialog();
        UpdateDGUsersInfo();
    }

    private void CbOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateDGUsersInfo();
    }
    public static int GetAge(DateOnly birthDate)
    {
        var now = DateTime.Today;
        return now.Year - birthDate.Year - 1 +
               ((now.Month > birthDate.Month || now.Month == birthDate.Month && now.Day >= birthDate.Day) ? 1 : 0);
    }

    private void BtnChangeRole_Click(object sender, RoutedEventArgs e)
    {
        if (DgUsersInfo.SelectedItem == null) return;
        User userR;
        using (var bd = new Session106Context())
        {
            userR = bd.Users.First(u => u.Id == ((UserDG)DgUsersInfo.SelectedItem).Id);
            if (userR.Id == _user.Id)
            {
                UpdateDGUsersInfo();
                return;
            }
        }
        new EditRole(userR).ShowDialog();
        UpdateDGUsersInfo();
    }

    private void UpdateDGUsersInfo()
    {
        DgUsersInfo.Items.Clear();
        if (CbOffice.SelectedIndex == 0)
        {
            using (var db = new Session106Context())
            {
                db.Offices.Load();
                db.Users.Load();
                db.Roles.Load();
                var users = db.Users.Local.ToBindingList();
                List<UserDG> usersList = new List<UserDG>();
                foreach (var user in users)
                {
                    var userDG = new UserDG
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = GetAge((DateOnly)user.Birthdate),
                        Role = user.Role.Id == 1 ? "administrator" : "office user",
                        Email = user.Email,
                        Office = user.Office,
                        Active = user.Active
                    };

                    usersList.Add(userDG);
                }
                foreach (var user in usersList)
                {
                    DgUsersInfo.Items.Add(user);
                }
            }
        }
        else
        {
            using (var db = new Session106Context())
            {
                var office = db.Offices.Where(o => o.Title == CbOffice.SelectedItem.ToString()).ToList()[0].Id;
                db.Users.Where(u => u.OfficeId == office).Load();
                db.Roles.Load();
                var users = db.Users.Local.ToBindingList();
                List<UserDG> usersList = new List<UserDG>();
                foreach (var user in users)
                {
                    var userDG = new UserDG
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = GetAge((DateOnly)user.Birthdate),
                        Role = user.Role.Id == 1 ? "administrator" : "office user",
                        Email = user.Email,
                        Office = user.Office,
                        Active = user.Active
                    };
                    usersList.Add(userDG);
                }
                foreach (var user in usersList)
                {
                    DgUsersInfo.Items.Add(user);
                }
            }
        }
    }

    private void BtnEnableDisableLogin_Click(object sender, RoutedEventArgs e)
    {
        if (DgUsersInfo.SelectedItem == null) return;
        User userR;
        using (var bd = new Session106Context())
        {
            userR = bd.Users.First(u => u.Id == ((UserDG)DgUsersInfo.SelectedItem).Id);
            if (userR.Id == _user.Id)
            {
                UpdateDGUsersInfo();
                return;
            }
            userR.Active = !(userR.Active);
            bd.Users.Update(userR);
            bd.SaveChanges();
        }
        UpdateDGUsersInfo();
    }
}