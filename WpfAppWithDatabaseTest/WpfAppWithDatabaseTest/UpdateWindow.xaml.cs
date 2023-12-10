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
