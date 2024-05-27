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

        private void LoadFreelances()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var freelances = context.Freelances
                    .Include(p => p.FreelanceCompetences)
                    .ThenInclude(pc => pc.IdCompetencesNavigation)
                    .ToList();
                allfreelance = new ObservableCollection<Freelance>(freelances);
            }
        }

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
                { "Name", "Prénom" },
                { "Surname", "Nom" },
                { "Description", "Description" },
                { "Competence", "Compétences" },
            };

            // Loop through columns to create DataGrid columns
            foreach (var column in columnsToDisplay)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn
                {
                    Header = column.Value,
                    Binding = new Binding(column.Key)
                };
                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(50); // Set width for ID column
                        break;
                    case "Name":
                    case "Surname":
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Auto); // Set width to Auto for Name, Surname, and Description columns
                        dataColumn.MinWidth = 150; // Set minimum width to 150 pixels
                        break;
                    case "Description":
                        dataColumn.Width = new DataGridLength(200); // Set width for ID column
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

        // Ajoutez une méthode pour recharger les données et actualiser le DataGrid
        private void RefreshDataGrid()
        {
            // Rechargez les freelances depuis la base de données
            LoadFreelances();

            // Actualisez le DataGrid en redéfinissant sa source de données
            dgFreelances.ItemsSource = allfreelance;
        }

        // Event handler for Add Button click
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Show AddFreelance window
            AddFreelance addFreelance = new AddFreelance();
            addFreelance.Closed += (s, args) => RefreshDataGrid(); // Actualisez le DataGrid après fermeture de la fenêtre d'ajout
            addFreelance.Show();
        }

        // Event handler for Remove Button click
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a freelance is selected
            if (SelectedFreelance != null)
            {
                using (MakeoProjectContext context = new())
                {
                    // Supprimez d'abord toutes les références au freelance dans la table freelance_projects
                    var freelanceProjects = context.FreelanceProjects.Where(fp => fp.FreelanceId == SelectedFreelance.Id).ToList();
                    context.FreelanceProjects.RemoveRange(freelanceProjects);
                    context.SaveChanges();

                    // Ensuite, supprimez le freelance lui-même
                    context.Freelances.Remove(SelectedFreelance);
                    context.SaveChanges();

                    // Retirez le freelance de la collection allfreelance
                    allfreelance.Remove(SelectedFreelance);

                    // Actualisez le DataGrid
                    RefreshDataGrid();

                    MessageBox.Show("Le freelance a bien été supprimé.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance à supprimer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for Edit Button click
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a freelance is selected and open the EditFreelance window passing the selected freelance data
            if (SelectedFreelance != null)
            {
                EditFreelance editFreelance = new EditFreelance(SelectedFreelance);
                editFreelance.Closed += (s, args) => RefreshDataGrid(); // Actualisez le DataGrid après fermeture de la fenêtre d'édition
                editFreelance.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance à modifier.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event handler for Info Button click
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgFreelances.SelectedItem is Freelance selectedFreelance)
            {
                var infoWindow = new InfoFreelance(selectedFreelance);
                infoWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance pour voir les détails.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
