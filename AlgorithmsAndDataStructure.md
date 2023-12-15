# Algorithms And Data Structures

## Narative

The artifact I chose to use for Algorithms and Data Structures comes from CS - 340 Client/Server Development. In the project, I used PyMongo to work with the MongoDB database. Also within this project, I had to create user and password variables to send as arguments in order to connect with the MongoDB client. For my enhancement, I wanted to remove the "in text" username and password variables and also include hashing to store the usernames and passwords in a datase and create a Login Window to access the Create, Update, and Delete functions in a separate Update Window.
#### ProjectTwoDashboard
In the begining of the program, you can see that the username and password are stored in plain text. These variables are then sent to the animalShelter.py program to connect to the MongoDB client to perform CRUD actions in MongoDB.
```
from jupyter_plotly_dash import JupyterDash

import dash
import dash_leaflet as dl
import dash_core_components as dcc
import dash_html_components as html
import plotly.express as px
import dash_table as dt
from dash.dependencies import Input, Output, State
from dash.exceptions import PreventUpdate

import os
import numpy as np
import pandas as pd
from pymongo import MongoClient
from bson.json_util import dumps

#### FIX ME #####
# change animal_shelter and AnimalShelter to match your CRUD Python module file name and class name
from animalShelter import AnimalShelter



###########################
# Data Manipulation / Model
###########################
# FIX ME change for your username and password and CRUD Python module name
username = "aacuser"
password = "aacuser123"
shelter = AnimalShelter(username, password)


# class read method must support return of cursor object 
df = pd.DataFrame.from_records(shelter.read({}))



#########################
# Dashboard Layout / View
#########################
app = JupyterDash('SimpleExample')

#FIX ME Add in Grazioso Salvare’s logo
#image_filename = 'Grazioso Salvare Logo.png' # replace with your own image
#encoded_image = base64.b64encode(open(image_filename, 'rb').read())

#FIX ME Place the HTML image tag in the line below into the app.layout code according to your design
#FIX ME Also remember to include a unique identifier such as your name or date
#html.Img(src='data:image/png;base64,{}'.format(encoded_image.decode()))

app.layout = html.Div([
#    html.Div(id='hidden-div', style={'display':'none'}),
    html.Header("Mark Jumper"),
#    html.Img(src=encoded_image),
    html.Center(html.B(html.H1('SNHU CS-340 Dashboard'))),
    html.Hr(),
    html.Div([
        
#FIXME Add in code for the interactive filtering options. For example, Radio buttons, drop down, checkboxes, etc.
    dcc.RadioItems(
        id='rescue-radio',
        options = [
            {'label': 'Water Rescue', 'value':'wtr'},
            {'label': 'Mountain Rescue', 'value':'mnt'},
            {'label': 'Disaster Rescue', 'value': 'dst'},
            {'label': 'Reset', 'value':'rst'}
            
        ],
        value='rst'),
#     dcc.Store(id='store-data', data=[], storage_type='memory')  
    ]),
    html.Hr(),
#     html.Div(id='table',children=[]),
    dt.DataTable(
        id='datatable-id',
        columns=[
            {"name": i, "id": i, "deletable": False, "selectable": True} for i in df.columns
        ],
        data=df.to_dict('records'),
#FIXME: Set up the features for your interactive data table to make it user-friendly for your client
#If you completed the Module Six Assignment, you can copy in the code you created here 
        editable=True,
        filter_action="native",
        sort_action="native",
        sort_mode="multi",
        page_current=0,
        page_size=10,
    ),
    html.Br(),
    html.Hr(),
#This sets up the dashboard so that your chart and your geolocation chart are side-by-side
    html.Div(className='row',
         style={'display' : 'flex'},
             children=[
        html.Div(
            id='graph-id',
            className='col s12 m6',

            ),
        html.Div(
            id='map-id',
            className='col s12 m6',
            )
        ])
])

#############################################
# Interaction Between Components / Controller
#############################################



    
@app.callback(
    Output('table', 'children'),
    [Input('rescue-radio', 'value')],
    [State('table','children')]
)
def update_chart(val, children):
    if val == 'wtr':
        df = pd.DataFrame.from_records(shelter.read({"$and":[
                {'breed':{"$in": ['Labrador Retriever Mix',
                                  'Chesapeake Bay Retriever',
                                  'Newfoundland']}},
                {'sex_upon_outcome':'Intact Female'},
                {'age_upon_outcome_in_weeks':{"$gt":26,"$lt":156}}
        ]}))
        
    elif val == 'mnt':
        df = pd.DataFrame.from_records(shelter.read({"$and":[
                   {'breed': {"$in": ['German Shepherd',
                                      'Alaskan Malamute',
                                      'Old English Sheepdog',
                                      'Siberian Husky',
                                      'Rottweiler']}},
                   {'sex_upon_outcome':'Intact Male'},
                   {'age_upon_outcome_in_weeks':{"$gt":26,"$lt":156}}]}))
                
    elif val == 'dst':
        df = pd.DataFrame.from_records(shelter.read(
                    {"$and":[
                        {'breed':{"$in":['Doberman Pinscher',
                                         'German Shepherd',
                                         'Golden Retriever',
                                         'Bloodhound',
                                         'Rottweiler']}},
                        {'sex_upon_outcome':'Intact Male'},
                        {'age_upon_outcome_in_weeks':{"$gt":20,"$lt":300}}
                    ]}
                ))
            
    elif val == 'rst':
        df = pd.DataFrame.from_records(shelter.read({}))
               
    return [
        dash_table.DataTable(
            id='datatable-id',
            columns=[
                {"name": i, "id": i, "deletable": False, "selectable": True} for i in df.columns
            ],
            data=df.to_dict('records'),
            #FIXME: Set up the features for your interactive data table to make it user-friendly for your client
            editable=True,
            filter_action="native",
            sort_action="native",
            sort_mode="multi",
            page_current=0,
            page_size=20,
        )                                                           
        ]


app
```
## Enhancements
For my enhancements, I took the LoginWindow that I created for the Software Engineering/Design to first query the database based on user login and password. The username (email) is queried first, then I created a stored procedure to take the password from user input and hash it to compare the password stored in the database. If these match, the user object is then used to see the role of the returned user to determine what CRUD operations are available.
#### Test Data with Hashed Passwords
```
INSERT INTO [Users] (FirstName, LastName, Email, Password, role)
VALUES ('Admin', 'Admin', 'Admin@Test.com', HASHBYTES('SHA2_256', N'Admin123'), 4),
	   ('Manager', 'Manager', 'Manager@Test.com', HASHBYTES('SHA2_256', N'Manager123'), 3),
	   ('Supervisor', 'Supervisor', 'Supervisor@Test.com', HASHBYTES('SHA2_256', N'Supervisor123'), 2),
	   ('Employee', 'Employee', 'Employee@Test.com', HASHBYTES('SHA2_256', N'Employee123'), 1)
```
![image](https://github.com/majumper87/majumper87.github.io/assets/75309837/c681b0aa-11d1-4ab9-b66e-d3dc2786162b)

#### T-SQL Stored Procedure to Convert and Check Passwords
```
CREATE PROCEDURE [dbo].[sp_CheckPassword]
	@param1 nvarchar(50) = ''
AS
BEGIN
DECLARE @hash nvarchar(50);
SET @hash = HASHBYTES('SHA2_256', @param1);
	SELECT @hash
RETURN 0
END
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
