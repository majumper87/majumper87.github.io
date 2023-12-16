[README.md](https://github.com/majumper87/majumper87.github.io/blob/main/README.md) - [Code Review](https://youtu.be/V6MCl8RoXNo) - [Software Engineering/Design](https://github.com/majumper87/majumper87.github.io/blob/main/SoftwareEngineeringAndDesign.md) - [Algorithms and Data Structures](https://github.com/majumper87/majumper87.github.io/blob/main/AlgorithmsAndDataStructure.md) - [Databases](https://github.com/majumper87/majumper87.github.io/blob/main/Databases.md)

# Databases

## Narative

The project I chose to enhance was from CS - 340 Client/Server Development. In this course, I learned to work with MongoDB, then implement what I learned to create a Python program to make a connection with the database and perform CRUD methods. For this, I used PyMongo, a library for working with Python and MongoDB. For my enhancements, I created a Windows Platform Foundation application that has a T-SQL database that accompanies the application. The application includes the main Animal Shelter database used for the CS - 340 project, along with a User table for managing users and user permissions. The user permissions are the main focus of my enhancement for utilizing databases by also creating a Login Window that returns user permissions.

## T-SQL User Table
For my enhancements, I used SSMS/T-SQL instead of MongoDB. The main enhancements was to include and implement user roles to determine the level of operations available to the user that is logged in. To do this I created a login screen to query the users in the database and return their role level – level 4 is admin, 3 managers, 2 supervisors, 1 regular employees or users. 

```
CREATE TABLE [dbo].[Users] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NULL,
    [LastName]  NVARCHAR (50) NULL,
    [Email]     NVARCHAR (50) NULL,
    [Password]  NVARCHAR (50) NULL,
    [Role]      TINYINT       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

After creating the user table, I filled in the table with test data to work with the Login Window.

```
INSERT INTO [Users] (FirstName, LastName, Email, Password, role)
VALUES ('Admin', 'Admin', 'Admin@Test.com', HASHBYTES('SHA2_256', N'Admin123'), 4),
	   ('Manager', 'Manager', 'Manager@Test.com', HASHBYTES('SHA2_256', N'Manager123'), 3),
	   ('Supervisor', 'Supervisor', 'Supervisor@Test.com', HASHBYTES('SHA2_256', N'Supervisor123'), 2),
	   ('Employee', 'Employee', 'Employee@Test.com', HASHBYTES('SHA2_256', N'Employee123'), 1)
```

## Login Window
In the WPF application, LoginWindow.xaml.cs, I implement this by using if/else statements to evaluate the user’s role in the user table to determine the elements available to that user. These include the visibility and use of either the delete, update, or add buttons in the UpdateWindow.xaml.cs before opening the window.
#### LoginWindow.xaml
```
<Window x:Class="WpfAppWithDatabaseTest.LoginWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfAppWithDatabaseTest"
        mc:Ignorable="d"
        Title="Login" Height="450" Width="800" FontSize="14" Background="BlanchedAlmond">
    <Border Background="Gray" CornerRadius="20" Margin="20">
        <StackPanel Margin="20">
            <Label Content="Login" Foreground="White" FontSize="25" HorizontalAlignment="Center"/>
            <Separator></Separator>
            <Label Content="Username" Foreground="White"/>
            <TextBox Name="txtUserName" Background="#545d6a" Foreground="White" FontSize="22"/>
            <Label Content="Password" Foreground="White"/>
            <PasswordBox Name="txtPassword" Background="#545d6a" Foreground="White" FontSize="22"/>
            <Button Name="btnSubmit" Content="Submit" Click="btnSubmit_Click" Margin="200 20" Background="#545d6a" Foreground="White" FontSize="22"/>
            <Button Name="btnRegister" Content="Register" Click="btnRegister_Click" Margin="200 0" Background="#545d6a" Foreground="White" FontSize="22"/>
        </StackPanel>
    </Border>
</Window>
```
#### LoginWindow.xaml.cs
```
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
```
