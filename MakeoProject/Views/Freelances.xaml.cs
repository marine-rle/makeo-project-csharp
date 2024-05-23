using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MakeoProject.Views
{
    /// <summary>
    /// Interaction logic for Freelances.xaml
    /// </summary>
    public partial class Freelances : UserControl
    {
        // ObservableCollection to store all freelances
        public ObservableCollection<Freelance> allfreelance { get; set; }

        // Property to store the selected freelance
        public Freelance? SelectedFreelance { get; set; }

        // Constructor
        public Freelances()
        {
            InitializeComponent();

            // Set DataContext to this instance
            this.DataContext = this;

            // Fetch all freelances from the database using Entity Framework Core
            using (MakeoProjectContext allfreelances = new())
            {
                allfreelance = new ObservableCollection<Freelance>(allfreelances.Freelances.Include(p => p.FreelanceCompetences).ThenInclude(pc => pc.IdCompetencesNavigation).ToList());
            }

            // Define columns to display in the DataGrid
            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "First Name" },
                { "Surname", "Last Name" },
                { "Description", "Description" },
                { "Competence", "Skills" },
            };

            // Loop through columns to create DataGrid columns
            foreach (var column in columnsToDisplay)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;
                dataColumn.Binding = new Binding(column.Key);
                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(50); // Set width for ID column
                        break;
                    case "Name":
                    case "Surname":
                    case "Description":
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Auto); // Set width to Auto for Name, Surname, and Description columns
                        dataColumn.MinWidth = 150; // Set minimum width to 150 pixels
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star); // Set width to Star for remaining columns
                        dataColumn.Binding = new Binding("Skills"); // Bind Skills property for Competence column
                        break;
                }
                dgFreelances.Columns.Add(dataColumn); // Add the created column to DataGrid
            }

            dgFreelances.AutoGenerateColumns = false; // Disable auto generation of columns
        }

        // Event handler for Add Button click
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Show AddFreelance window
            AddFreelance addFreelance = new AddFreelance();
            addFreelance.Show();
        }

        // Event handler for Remove Button click
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a freelance is selected and remove it from the database and ObservableCollection
            if (this.allfreelance != null && SelectedFreelance != null)
            {
                using (MakeoProjectContext context = new())
                {
                    context.Freelances.Remove(SelectedFreelance);
                    context.SaveChanges();
                    this.allfreelance.Remove(SelectedFreelance);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a freelance is selected and open the EditFreelance window passing the selected freelance data
            if (SelectedFreelance != null)
            {
                EditFreelance editFreelance = new EditFreelance(SelectedFreelance);
                editFreelance.Show();
            }
        }


    }
}