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

namespace WpfAppWithDatabaseTest
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (isValidated())
            {
                AnimalShelterEntities db = new AnimalShelterEntities();
                var r = from d in db.Users
                        where d.Email.Equals(txtUserName.Text)
                        select d.Password.ToString();

                MessageBox.Show(r.ToString());        

            }
            else
            {
                MessageBox.Show("wrong password");
            }
            */
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool isValidated()
        {
            if(txtUserName.Text == string.Empty)
            {
                MessageBox.Show("Enter Username");
                txtUserName.Clear();
                txtPassword.Clear();
                txtUserName.Focus();
                return false;
            }
            if (txtPassword.Password == string.Empty)
            {
                MessageBox.Show("Enter Username");
                txtPassword.Clear();
                txtUserName.Focus();
                return false;
            }
            return true;
        }
    }
}
