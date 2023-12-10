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
