using Microsoft.EntityFrameworkCore;
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

namespace WPFDigitalSkills2017Core
{
    /// <summary>
    /// Логика взаимодействия для EditRole.xaml
    /// </summary>
    public partial class EditRole : Window
    {
        public User _user
        {
            get;
            set;
        }
        public EditRole(User user)
        {
            _user = user;
            InitializeComponent();
            TbEmail.Text = _user.Email;
            TbFirstName.Text = _user.FirstName;
            TbLastName.Text = _user.LastName;
            using (var bd = new Session106Context())
            {
                bd.Offices.Load();
                bd.Roles.Load();
                var offices = bd.Offices.Local.ToBindingList();
                foreach (var office in offices)
                {
                    CbOffices.Items.Add(office.Title);
                }

                var officeTitle = bd.Offices.First(o => o.Id == _user.OfficeId);
                CbOffices.SelectedIndex = CbOffices.Items.IndexOf(officeTitle.Title);
                if (_user.RoleId == 1) RbntAdministrator.IsChecked = true;
                if (_user.RoleId == 2) RbntUser.IsChecked = true;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();

        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            _user.RoleId = RbntAdministrator.IsChecked == true ? 1 : 2;
            using (var bd = new Session106Context())
            {
                bd.Users.Update(_user);
                bd.SaveChanges();
            }

            DialogResult = true;
        }
    }
}
