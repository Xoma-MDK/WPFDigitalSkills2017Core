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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using WPFDigitalSkills2017Core.Models;

namespace WPFDigitalSkills2017Core;

public partial class MainWindow : Window
{
    private byte attempts = 3;
    private bool permitLogin = true;
    DispatcherTimer dispatcherTimer = new DispatcherTimer();
    private byte timer = 10;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        Login();
    }
    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
        if (timer == 0)
        {
            dispatcherTimer.Stop();
            attempts = 3;
            timer = 10;
            LbErrorAndTimer.Foreground = Brushes.Green;
            LbErrorAndTimer.Content = $"Try again!";
            permitLogin = true;
        }
        else
        {
            LbErrorAndTimer.Content = $"Please wait {timer--} second";
        }
    }
    public static string CreateMd5(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void PbPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Login();
        }
    }

    private void TbUserName_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            PbPassword.Focus();
        }
    }

    private void Login()
    {
        using (var bd = new Session106Context())
        {
            if (!permitLogin) return;
            var user = bd.Users.FirstOrDefault(u => u.Email == TbUserName.Text && u.Password == CreateMd5(PbPassword.Password));

            if (user != null)
            {
                if (user.Active == false)
                {
                    LbErrorAndTimer.Foreground = Brushes.Red;
                    LbErrorAndTimer.Content = "Sorry! Your account has been blocked by the administrator.";
                    return;
                }
                try
                {
                    var activityOfUse = bd.Activityofuses.Where(a => a.UserId == user.Id).OrderBy(a => a.LoginTime)
                        .Last();

                    if (activityOfUse.LogoutTime < activityOfUse.LoginTime && activityOfUse.Reason == String.Empty)
                    {
                        new NoLogoutDetected(user, activityOfUse).Show();
                        Close();
                    }
                    else
                    {
                        var activityOfUseNote = new Activityofuse
                        {
                            UserId = bd.Users.Where(u =>
                                    u.Email == TbUserName.Text && u.Password == CreateMd5(PbPassword.Password))
                                .ToList()[0].Id,
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            LoginTime = DateTime.Now,
                            Reason = ""
                        };
                        bd.Activityofuses.Add(activityOfUseNote);
                        bd.SaveChanges();
                        if (user.RoleId == 1)
                        {
                            new AdminScreen(user, activityOfUseNote).Show();
                            Close();
                        }
                        else if (user.RoleId == 2)
                        {
                            new UserScreen(user, activityOfUseNote).Show();
                            Close();
                        }
                    }
                }
                catch
                {
                    var activityOfUseNote = new Activityofuse
                    {
                        UserId = bd.Users.Where(u =>
                                u.Email == TbUserName.Text && u.Password == CreateMd5(PbPassword.Password))
                            .ToList()[0].Id,
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        LoginTime = DateTime.Now,
                        Reason = ""
                    };
                    bd.Activityofuses.Add(activityOfUseNote);
                    bd.SaveChanges();
                    if (user.RoleId == 1)
                    {
                        new AdminScreen(user, activityOfUseNote).Show();
                        this.Close();
                    }
                    else if (user.RoleId == 2)
                    {
                        new UserScreen(user, activityOfUseNote).Show();
                        this.Close();
                    }
                }
            }
            else
            {
                if (attempts <= 1)
                {
                    permitLogin = false;
                    dispatcherTimer.Tick += dispatcherTimer_Tick;
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                    dispatcherTimer.Start();
                }
                else
                {
                    attempts--;
                    LbErrorAndTimer.Foreground = Brushes.Red;
                    LbErrorAndTimer.Content = $"Invalid username or password! Try again. Attempts left:{attempts}";
                }
            }
        }
    }
}
