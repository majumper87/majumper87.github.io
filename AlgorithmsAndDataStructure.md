# Algorithms And Data Structures

## Narative

The artifact I chose to use for Algorithms and Data Structures comes from CS - 340 Client/Server Development. In the project, I used PyMongo to work with the MongoDB database. Also within this project, I had to create a user/password to connect with the MongoDB. For my enhancement, I wanted to remove the "in text" username and password variables and also include hashing to store the user passwords.
#### ProjectTwoDashboard
In the begining of the program, you can see that the username and password are stored in plain text.
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

