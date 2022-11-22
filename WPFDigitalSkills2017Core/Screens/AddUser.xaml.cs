using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPFDigitalSkills2017Core
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
            using (var bd = new Session106Context())
            {
                bd.Offices.Load();
                var offices = bd.Offices.Local.ToBindingList();
                foreach (var office in offices)
                {
                    CbOffices.Items.Add(office.Title);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(TbEmail.Text);
            if (match.Success)
            {
                using (var db = new Session106Context())
                {
                    if (db.Users.Any(u => u.Email == TbEmail.Text)) return;
                    if (CbOffices.SelectionBoxItemStringFormat == String.Empty) return;
                    if (DpBirthdate.SelectedDate == null) return;
                    var officeId = db.Offices.First(o => o.Title == CbOffices.SelectedItem.ToString()).Id;
                    var user = new User
                    {
                        FirstName = TbFirstName.Text,
                        LastName = TbLastName.Text,
                        Email = TbEmail.Text,
                        Password = CreateMd5(PbPassword.Password),
                        RoleId = 2,
                        Active = true,
                        OfficeId = officeId,
                        Birthdate = DateOnly.FromDateTime((DateTime)DpBirthdate.SelectedDate)
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    DialogResult = true;
                    Close();
                }

            }
            
        }
        public static string CreateMd5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
