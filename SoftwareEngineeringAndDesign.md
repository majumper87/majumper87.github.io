[README.md](https://github.com/majumper87/majumper87.github.io/blob/main/README.md) - [Code Review](https://youtu.be/V6MCl8RoXNo) - [Software Engineering/Design](https://github.com/majumper87/majumper87.github.io/blob/main/SoftwareEngineeringAndDesign.md) - [Algorithms and Data Structures](https://github.com/majumper87/majumper87.github.io/blob/main/AlgorithmsAndDataStructure.md) - [Databases](https://github.com/majumper87/majumper87.github.io/blob/main/Databases.md)

# Software Engineering and Design
## Narative
The project that I will be using is from CS 340 - Client/Server Development. For the final project for this course, we were tasked with creating a Graphical User Interface using Dash Plotly. Dash Plotly is a library for working with Python that includes HTML tags for creating a layout. With this layout, there is a header, radio buttons and a grid for displaying information from the database used for the project. This is related to Software Engineering and Design in that the layout was designed around filtering and displaying specific information for the "Customer".
## CS-340 Project Two Dashboard
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

@app.callback(
    Output('datatable-id', 'style_data_conditional'),
    [Input('datatable-id', 'selected_columns')]
)
def update_styles(selected_columns):
    return [{
        'if': { 'column_id': i },
        'background_color': '#D2F3FF'
    } for i in selected_columns]

@app.callback(
    Output('graph-id', "children"),
    [Input('datatable-id', "derived_viewport_data")])
def update_graphs(viewData):
    ###FIX ME ####
    # add code for chart of your choice (e.g. pie chart) #
    dff = pd.DataFrame.from_dict(viewData)
    return [
        dcc.Graph(
            
            figure = px.scatter(dff, x = "age_upon_outcome_in_weeks",y="age_upon_outcome_in_weeks", color="breed")
        )    
    ]

@app.callback(
    Output('map-id', "children"),
    [Input('datatable-id', "derived_viewport_data")]
)
def update_map(viewData):
#FIXME: Add in the code for your geolocation chart
    dff = pd.DataFrame.from_dict(viewData)
    # Austin TX is at [30.75,-97.48]
    return [
        dl.Map(style={'width': '1000px', 'height': '500px'}, center=[30.75,-97.48], zoom=10, children=[
            dl.TileLayer(id="base-layer-id"),
            # Marker with tool tip and popup
            dl.Marker(position=[30.75,-97.48], children=[
                dl.Tooltip(dff.iloc[0,4]),
                dl.Popup([
                    html.H1("Animal Name"),
                    html.P(dff.iloc[1,9])
                ])
            ])
        ])
    ]                  
#If you completed the Module Six Assignment, you can copy in the code you created here.



app
```
![image](https://github.com/majumper87/majumper87.github.io/assets/75309837/b672230d-c537-4c7d-99a4-efa8e0ed70ae)


# Enhancements
This artifact was created early in 2023. It was created to work with the Python libraries for Dash Plotly and MongoDB with PyMongo. The enhancements that I chose to make was to recreate the project using Windows Presentation Foundation (WPF). Among these enhancements was to create a window that displayed the information of the database/.csv file. The other enhancements were to provide to the vendor their specific filters for rescue animals. I accomplished this by creating SQL queries using stored procedures to display to the DataGrid when the relative button is pressed. The second window is to perform other CRUD operations. The third is to provide a login window that determines user roles before accessing the specific CRUD operations.

I decided to create this artifact because most of my career has revolved around Windows Operating systems. I wanted to build an application that could demonstrate software that could be utilized locally for a specific site using this operating system. Much like stand-alone software, one customer could be looking for a specific criterion, while another could be looking for something else. This software would be specific to the customer and their specific location/needs or their customers.

As where the main window displays and filters records, the “Update” window utilizes a search feature based on the Animal ID as it is unique to each. Each textbox is assigned to it’s own field and allows editing. In addition, for adding additional animals, there is a conditional statement that checks to make sure that the Animal ID is unique, or else displays a window prompt to let the user know that the ID already exists. For the delete function, the user is prompted with an “are you sure” message before deletion.

Most of the objectives in my enhancement plan have been finished by re-creating the display windows and the ability to perform CRUD operations. The only other update that has not been completed was to create and implement a user login solution. This window would be placed in the middle of the update window to perform the CRUD operations. As well, I would need to create a window to add additional users and user roles, along with implementing further conditionals within the code to query these users and roles.

Some of the things I have learned from creating this artifact has been utilizing the DataGrid in WPF along with passing information to the grid by utilizing stored procedures in the database and importing them into the application. The next step was to utilize Entity Framework to create the collection(s) that then get returned to the grid. In addition, with displaying individual components of a single document to an associated textbox that can then also be used to edit the SQL database. Some of the challenges with this was to create the window, name all of the textboxes and then figuring out how to return a single item from the collection to an individual textbox.

MainWindow.xaml - in this codeblock, I show the layout that is used for the application.
```
<Window x:Class="WpfAppWithDatabaseTest.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfAppWithDatabaseTest"
        mc:Ignorable="d"
        Title="Austin Animal Shelter" Height="900" Width="1200" Background="BlanchedAlmond">
    <Grid>
        <DataGrid x:Name="shelterData" Margin="0,217,0.333,-0.333" Width="auto"/>
        <Label x:Name="header" Content="Austin Animal Shelter" FontSize="38" HorizontalAlignment="Center" Margin="0,40,0,0" VerticalAlignment="Top" Height="auto" Width="auto"/>
        <Button x:Name="waterButton" Content="Water Rescue" Click="waterButton_Click" FontSize="14" HorizontalAlignment="Left" Margin="10,195,0,0" VerticalAlignment="Top" Width="auto"/>
        <Button x:Name="mountainButton" Content="Mountain Rescue" Click="mountainButton_Click" FontSize="14" HorizontalAlignment="Left" Margin="118,195,0,0" VerticalAlignment="Top" Width="auto"/>
        <Button x:Name="disasterButton" Content="Disaster Rescue" Click="disasterButton_Click" FontSize="14" HorizontalAlignment="Left" Margin="249,195,0,0" VerticalAlignment="Top" Width="auto"/>
        <Button x:Name="refreshButton" Content="Refresh" Click="refreshButton_Click" FontSize="14" HorizontalAlignment="Left" Margin="369,195,0,0" VerticalAlignment="Top" Width="auto"/>
        <Button x:Name="updateBtn" Content="Update" Click="updateButton_Click"  HorizontalAlignment="Left" Margin="1108,191,0,0" VerticalAlignment="Top" Width="75"/>

    </Grid>
</Window>
```
MainWindow.xaml.cs - This codeblock shows the underlying C# that performs the actions of the initial window display, and actions performed by button clicks.
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WpfAppWithDatabaseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            // Initializes DataGrid
            AnimalShelterEntities db = new AnimalShelterEntities();

            var r = from d in db.spGetAnimals1()
                    select d;

            this.shelterData.ItemsSource = r.ToList();
            
        }

        // This is the function for the Water button that returns the Water Rescue animals
        private void waterButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize DB and create variable to hold Stored Procedure Colletion
            AnimalShelterEntities db = new AnimalShelterEntities();
            var r = from d in db.spGetWaterRescue()
                    select d;
            shelterData.ItemsSource = r.ToList();
        }

        // This is the function for the Mountain button that returns the Mountain/Wilderness Rescue animals
        private void mountainButton_Click(Object sender, RoutedEventArgs e)
        {
            // Initialize DB and create variable to hold Stored Procedure Colletion
            AnimalShelterEntities db = new AnimalShelterEntities();
            var r = from d in db.spGetMountainWildernessRescue()
                    select d;
            shelterData.ItemsSource = r.ToList();
        }

        // This is the function for the Disaster button that returns the Disaster Rescue animals
        private void disasterButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize DB and create variable to hold Stored Procedure Colletion
            AnimalShelterEntities db = new AnimalShelterEntities();
            var r = from d in db.spGetDisasterIndividualRescue()
                    select d;
            shelterData.ItemsSource = r.ToList();
        }

        // Refresh button to return all animals
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize DB and create variable to hold Stored Procedure Colletion
            AnimalShelterEntities db = new AnimalShelterEntities();
            var r = from d in db.spGetAnimals1()
                    select d;
            shelterData.ItemsSource = r.ToList();
        }

        // This opens the update menue
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // Create new object of the UpdateWindow class and open window.
            //UpdateWindow update = new UpdateWindow();
            //update.Show();
            LoginWindow login = new LoginWindow();
            login.Show();
           
        }
    }
}

```

UpdateWindow.xaml - This is the layout of the update window
```
<Window x:Class="WpfAppWithDatabaseTest.UpdateWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfAppWithDatabaseTest"
        mc:Ignorable="d"
        Title="UpdateWindow" Height="600" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="*"></RowDefinition>
        </Grid.RowDefinitions>

        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto"></ColumnDefinition>
                <ColumnDefinition></ColumnDefinition>
                <ColumnDefinition Width="Auto"></ColumnDefinition>
                <ColumnDefinition Width="Auto"></ColumnDefinition>
                <ColumnDefinition Width="Auto"></ColumnDefinition>
            </Grid.ColumnDefinitions>
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"></RowDefinition>
            </Grid.RowDefinitions>

            <TextBlock Margin="7">Animal ID:</TextBlock>
            <TextBox x:Name="txtID" Margin="5" Grid.Column="1"></TextBox>
            <Button x:Name="btnGetAnimal" Click="btnGetAnimal_Click" Margin="5" Padding="2" Grid.Column="2">Get Animal</Button>
            <Button x:Name="btnUpdateAnimal" Click="btnUpdateAnimal_Click" Margin="5" Padding="2" Grid.Column="3">Update Animal</Button>
            <Button x:Name="btnAddAnimal" Click="btnAddAnimal_Click" Margin="5" Padding="2" Grid.Column="4">Add New Animal</Button>
        </Grid>

        <Border Grid.Row="1" Padding="7" Margin="7" Background="BlanchedAlmond">
            <Grid Name="gridAnimalDetails">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"></ColumnDefinition>
                    <ColumnDefinition></ColumnDefinition>
                </Grid.ColumnDefinitions>
                
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    <RowDefinition Height="Auto"></RowDefinition>
                    
                                    
                </Grid.RowDefinitions>
                
                <TextBlock Margin="7">Age Upon Outcome:</TextBlock>
                <TextBox x:Name="txtAgeUponOutcome" Margin="5" Grid.Column="1"
                    Text=""></TextBox>
            
                <TextBlock Margin="7" Grid.Row="1">Animal ID:</TextBlock>
                <TextBox x:Name="txtAnimalId" Margin="5" Grid.Row="1" Grid.Column="1"
                    Text="{Binding Path=animal_id}"></TextBox>
            
                <TextBlock Margin="7" Grid.Row="2">Animal Type:</TextBlock>
                <TextBox x:Name="txtAnimalType" Margin="5" Grid.Row="2" Grid.Column="1"
                    Text="{Binding Path=animal_type}"></TextBox>
            
                <TextBlock Margin="7" Grid.Row="3">Breed:</TextBlock>
                <TextBox x:Name="txtBreed" Margin="5" Grid.Row="3" Grid.Column="1"
                    Text="{Binding Path=breed}"></TextBox>
            
                <TextBlock Margin="7" Grid.Row="4">Color:</TextBlock>
                <TextBox x:Name="txtColor" Margin="5" Grid.Row="4" Grid.Column="1"
                    Text="{Binding Path=color}"></TextBox>

                <TextBlock Margin="7" Grid.Row="5">Date of Birth:</TextBlock>
                <TextBox x:Name="txtDateOfBirth" Margin="5" Grid.Row="5" Grid.Column="1"
                    Text="{Binding Path=date_of_birth}"></TextBox>

                <TextBlock Margin="7" Grid.Row="6">DateTime:</TextBlock>
                <TextBox x:Name="txtDateTime" Margin="5" Grid.Row="6" Grid.Column="1"
                    Text="{Binding Path=datetime}"></TextBox>

                <TextBlock Margin="7" Grid.Row="7">Month/Year:</TextBlock>
                <TextBox x:Name="txtMonthYear" Margin="5" Grid.Row="7" Grid.Column="1"
                    Text="{Binding Path=monthyear}"></TextBox>

                <TextBlock Margin="7" Grid.Row="8">Name:</TextBlock>
                <TextBox x:Name="txtName" Margin="5" Grid.Row="8" Grid.Column="1"
                    Text="{Binding Path=name}"></TextBox>

                <TextBlock Margin="7" Grid.Row="9">Outcome Subtype:</TextBlock>
                <TextBox x:Name="txtOutcomeSubtype" Margin="5" Grid.Row="9" Grid.Column="1"
                    Text="{Binding Path=outcome_subtype}"></TextBox>

                <TextBlock Margin="7" Grid.Row="10">Outcome Type:</TextBlock>
                <TextBox x:Name="txtOutcomeType" Margin="5" Grid.Row="10" Grid.Column="1"
                    Text="{Binding Path=outcome_type}"></TextBox>

                <TextBlock Margin="7" Grid.Row="11">Sex Upon Outcome:</TextBlock>
                <TextBox x:Name="txtSexUponOutcome" Margin="5" Grid.Row="11" Grid.Column="1"
                    Text="{Binding Path=sex_upon_outcome}"></TextBox>

                <TextBlock Margin="7" Grid.Row="12">Location Lat:</TextBlock>
                <TextBox x:Name="txtLocLat" Margin="5" Grid.Row="12" Grid.Column="1"
                    Text="{Binding Path=location_lat}"></TextBox>

                <TextBlock Margin="7" Grid.Row="13">Location Long:</TextBlock>
                <TextBox x:Name="txtLocLong" Margin="5" Grid.Row="13" Grid.Column="1"
                    Text="{Binding Path=location_long}"></TextBox>

                <TextBlock Margin="7" Grid.Row="14">Age In Weeks:</TextBlock>
                <TextBox x:Name="txtAgeUponOutcomeInWeeks" Margin="5" Grid.Row="14" Grid.Column="1"
                    Text="{Binding Path=age_upon_outcome_in_weeks}"></TextBox>

                <Button Margin="30" Grid.Row="15" Grid.Column="2" x:Name="btnDelete" Content="Delete" FontSize="12"
                        Click="btnDeleteAnimal_Click" VerticalAlignment="Bottom" HorizontalAlignment="Center"></Button>
            </Grid>
        </Border>
    </Grid>
</Window>
```
UpdateWindow.xaml.cs - This codeblock shows the object reference to button clicks and the relation between the object, textblocks and CRUD methods
```
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Xml.Linq;

namespace WpfAppWithDatabaseTest
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        // GetAnimal Click Event
        private void btnGetAnimal_Click(object sender, RoutedEventArgs e)
        {
            // Initialize DB and create variable to hold Colletion to search datastore based off of Unique Animal ID
            AnimalShelterEntities db = new AnimalShelterEntities();

            var r = from d in db.aac_shelter_outcomes
                    where d.animal_id == txtID.Text
                    select d;

            // Return Animal to associated fields
            foreach (var item in r)
            {
                txtAgeUponOutcome.Text = item.age_upon_outcome.ToString();
                txtAnimalId.Text = item.animal_id.ToString();
                txtAnimalType.Text = item.animal_type.ToString();
                txtBreed.Text = item.breed.ToString();
                txtColor.Text = item.color.ToString();
                txtDateOfBirth.Text = item.date_of_birth.ToString();
                txtDateTime.Text = item.datetime.ToString();
                txtMonthYear.Text = item.monthyear.ToString();
                txtName.Text = item.name.ToString();
                txtOutcomeSubtype.Text = item.outcome_subtype.ToString();
                txtOutcomeType.Text = item.outcome_type.ToString();
                txtSexUponOutcome.Text = item.sex_upon_outcome.ToString();
                txtLocLat.Text = item.location_lat.ToString();
                txtLocLong.Text = item.location_long.ToString();
                txtAgeUponOutcomeInWeeks.Text = item.age_upon_outcome_in_weeks.ToString();

            }

        }

        // Update Click Event
        private void btnUpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            // Initialize DB and return row based on Animal ID
            AnimalShelterEntities db = new AnimalShelterEntities();

            var r = from d in db.aac_shelter_outcomes
                    where d.animal_id == txtID.Text
                    select d;

            // Make sure that object is a single record
            aac_shelter_outcomes obj = r.SingleOrDefault();

            // Make sure that we are working with an object and Return to associated fields
            if (obj != null)
            {
                obj.age_upon_outcome = txtAgeUponOutcome.Text;
                obj.animal_type = txtAnimalType.Text;
                obj.breed = txtBreed.Text;
                obj.color = txtColor.Text;
                obj.date_of_birth = txtDateOfBirth.Text;
                obj.datetime = txtDateTime.Text;
                obj.monthyear = txtMonthYear.Text;
                obj.name = txtName.Text;
                obj.outcome_subtype = txtOutcomeSubtype.Text;
                obj.outcome_type = txtOutcomeType.Text;
                obj.sex_upon_outcome = txtSexUponOutcome.Text;
                obj.location_lat = float.Parse(txtLocLat.Text);
                obj.location_long = float.Parse(txtLocLong.Text);
                obj.age_upon_outcome_in_weeks = float.Parse(txtAgeUponOutcomeInWeeks.Text);

                // Actual event to save any changes when button is clicked
                db.SaveChanges();
            }

            
        }

        private void btnAddAnimal_Click(object sender, RoutedEventArgs e)
        {
            
            //Dictionary<string, r > r = null;
            // Initialize DB and return single
            AnimalShelterEntities db = new AnimalShelterEntities();

            var r = from d in db.aac_shelter_outcomes
                    where (d.animal_id == txtID.Text || d.animal_id == txtAnimalId.Text)
                    select d;

            // Make sure returned is a single row
            aac_shelter_outcomes obj = r.SingleOrDefault();
            
            // FIXME: Testing for null pointer - make sure that "Animal ID" "search box" at top is populated
            /*
            MessageBox.Show(txtID.Text);
            MessageBox.Show(txtAnimalId.Text);
            MessageBox.Show(obj.animal_id);
            */
                        
            // Make sure that "New" Animal ID is Unique
            if (/*txtID.Text == "" || */ obj.animal_id != txtID.Text || obj.animal_id != txtAnimalId.Text)
            {
                aac_shelter_outcomes animal = new aac_shelter_outcomes()
                {
                    age_upon_outcome = txtAgeUponOutcome.Text,
                    animal_id = txtAnimalId.Text,
                    animal_type = txtAnimalType.Text,
                    breed = txtBreed.Text,
                    color = txtColor.Text,
                    date_of_birth = txtDateOfBirth.Text,
                    datetime = txtDateTime.Text,
                    monthyear = txtMonthYear.Text,
                    name = txtName.Text,
                    outcome_subtype = txtOutcomeSubtype.Text,
                    outcome_type = txtOutcomeType.Text,
                    sex_upon_outcome = txtSexUponOutcome.Text,
                    location_lat = float.Parse(txtLocLat.Text),
                    location_long = float.Parse(txtLocLong.Text),
                    age_upon_outcome_in_weeks = float.Parse(txtAgeUponOutcomeInWeeks.Text)

                };

                // Add animal to table and save changes
                db.aac_shelter_outcomes.Add(animal);
                db.SaveChanges();
            }
            else
            {
                // Let the user know if Animal ID is not unique
                MessageBox.Show("Not Unique Animal ID");
            }
                        
        }

        private void btnDeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            // Display message box to give the user an option to cancle if button was unintentionally pressed
            MessageBoxResult msg = MessageBox.Show("Are you sure you want to Delete record?",
                "Delete Animal record",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            // Conditional statement based on user Input if pressed "yes"
            if (msg == MessageBoxResult.Yes)
            {
                // Initialize and return single record based on Animal ID
                AnimalShelterEntities db = new AnimalShelterEntities();

                var r = from d in db.aac_shelter_outcomes
                        where d.animal_id == txtID.Text
                        select d;

                // Make sure it is a single record in the table
                aac_shelter_outcomes obj = r.SingleOrDefault();

                if (obj.animal_id == txtID.Text)
                {
                    db.aac_shelter_outcomes.Remove(obj);
                    db.SaveChanges();

                    // let user know that record was deleted
                    MessageBox.Show("Animal Record Deleted!");
                }
            }

        }
    }
}
```
