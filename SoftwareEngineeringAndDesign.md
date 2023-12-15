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

TODO: Add snippet of running dashboard here
# Enhancements
This artifact was created early in 2023. It was created to work with the Python libraries for Dash Plotly and MongoDB with PyMongo. The enhancements that I chose to make was to recreate the project using Windows Presentation Foundation (WPF). Among these enhancements was to create a window that displayed the information of the database/.csv file. The other enhancements were to provide to the vendor their specific filters for rescue animals. I accomplished this by creating SQL queries using stored procedures to display to the DataGrid when the relative button is pressed. The second window is to perform other CRUD operations. The third is to provide a login window that determines user roles before accessing the specific CRUD operations.

I decided to create this artifact because most of my career has revolved around Windows Operating systems. I wanted to build an application that could demonstrate software that could be utilized locally for a specific site using this operating system. Much like stand-alone software, one customer could be looking for a specific criterion, while another could be looking for something else. This software would be specific to the customer and their specific location/needs or their customers.

As where the main window displays and filters records, the “Update” window utilizes a search feature based on the Animal ID as it is unique to each. Each textbox is assigned to it’s own field and allows editing. In addition, for adding additional animals, there is a conditional statement that checks to make sure that the Animal ID is unique, or else displays a window prompt to let the user know that the ID already exists. For the delete function, the user is prompted with an “are you sure” message before deletion.

Most of the objectives in my enhancement plan have been finished by re-creating the display windows and the ability to perform CRUD operations. The only other update that has not been completed was to create and implement a user login solution. This window would be placed in the middle of the update window to perform the CRUD operations. As well, I would need to create a window to add additional users and user roles, along with implementing further conditionals within the code to query these users and roles.

Some of the things I have learned from creating this artifact has been utilizing the DataGrid in WPF along with passing information to the grid by utilizing stored procedures in the database and importing them into the application. The next step was to utilize Entity Framework to create the collection(s) that then get returned to the grid. In addition, with displaying individual components of a single document to an associated textbox that can then also be used to edit the SQL database. Some of the challenges with this was to create the window, name all of the textboxes and then figuring out how to return a single item from the collection to an individual textbox.

TODO: Add snippets of code and windows here 
