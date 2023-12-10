using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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

        /*
        public class UserRoleAuth
        {
            public AnimalShelterEntities roles = new AnimalShelterEntities()
            {

            }
            
        }
        */

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            if (isValidated())
            {
                // create the entity variable to return the row based on user email
                AnimalShelterEntities db = new AnimalShelterEntities();
                var r = from d in db.Users
                        where d.Email.Equals(txtUserName.Text)
                        select d;

                // create variable to hold string from passwordbox
                PasswordBox pwd = new PasswordBox();
                pwd.Password = txtPassword.Password;

                // create entity from passwordbox to use the stored procedure to create and return hashed value
                AnimalShelterEntities dbcheck = new AnimalShelterEntities();
                var psswd = from d in dbcheck.sp_CheckPassword(pwd.Password)
                            select d;

                // get returned hash from stored procedure to check user password hash
                string password = psswd.SingleOrDefault();

                // return user object to display text
                User obj = r.SingleOrDefault();
                
                // This section block was used for testing
                /*
                MessageBox.Show(pwd.Password);
                MessageBox.Show(password);
                MessageBox.Show(obj.Password);
                 */

                // check that obj is not null
                if (obj == null)
                {
                    MessageBox.Show("User not Recognized");
                }

                // make checks to verify that passwordbox hash equals hash for user stored in the user table, then open update window and close login window
                else
                {
                    if (obj.Password == password)
                    {
                        // Verify user role and Auth level for CRUD operations and button visibility - also disabled buttons to be sure to remove functionality
                        // 4 - Admin
                        // 3 - Manager
                        // 2 - Supervisor
                        // 1 - Employee

                        if (obj.Role == 4)
                        {
                            UpdateWindow update = new UpdateWindow();
                            update.Show();

                            this.Close();
                        }
                        else if (obj.Role == 3)
                        {
                            UpdateWindow update = new UpdateWindow();
                            update.btnDelete.IsEnabled = false;
                            update.btnDelete.Visibility = Visibility.Collapsed;
                            update.Show();

                            this.Close();
                        }
                        else if (obj.Role == 2)
                        {
                            UpdateWindow update = new UpdateWindow();
                            update.btnDelete.IsEnabled = false;
                            update.btnDelete.Visibility = Visibility.Collapsed;
                            update.btnUpdateAnimal.IsEnabled = false;
                            update.btnUpdateAnimal.Visibility = Visibility.Collapsed;
                            update.Show();

                            this.Close();
                        }
                        else if (obj.Role == 1)
                        {
                            UpdateWindow update = new UpdateWindow();
                            update.btnDelete.IsEnabled = false;
                            update.btnDelete.Visibility = Visibility.Collapsed;
                            update.btnUpdateAnimal.IsEnabled = false;
                            update.btnUpdateAnimal.Visibility = Visibility.Collapsed;
                            update.btnAddAnimal.IsEnabled = false;
                            update.btnAddAnimal.Visibility = Visibility.Collapsed;
                            
                            update.Show();

                            this.Close();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Wrong Password");
                    }
                }
            }
            else
            {
                MessageBox.Show("wrong password");
            }
            
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //TODO: create registration window
        }

        // this is to check that Username and Password fields are populated upon hitting the submit button
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
