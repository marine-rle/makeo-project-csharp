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
    public partial class Projects : UserControl
    {
        public ObservableCollection<Project> allproject { get; set; }
        public Project? SelectedProject { get; set; }

        public Projects()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadProjects();

            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Title", "Titre" },
                { "Description", "Description" },
                { "Competence", "Competence" },
                { "User", "Client" },
                { "DateDemande", "Date" },
                { "Statut", "Statut" }
            };

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
                        dataColumn.Width = new DataGridLength(30);
                        break;
                    case "Title":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    case "Description":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    case "Competence":
                        dataColumn.Width = new DataGridLength(150);
                        dataColumn.Binding = new Binding("Skills");
                        break;
                    case "User":
                        dataColumn.Width = new DataGridLength(150);
                        dataColumn.Binding = new Binding("UserName");
                        break;
                    case "DateDemande":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        dataColumn.Binding = new Binding("StatusName");
                        break;
                }
                dgProject.Columns.Add(dataColumn);
            }

            dgProject.AutoGenerateColumns = false;
        }

        private void LoadProjects()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var projects = context.Projects
                    .Include(p => p.User)
                    .Include(p => p.Statut)
                    .Include(p => p.ProjetCompetences)
                        .ThenInclude(pc => pc.IdCompetencesNavigation)
                    .Where(p => p.Statut.Name != "Brouillon")
                    .ToList();
                allproject = new ObservableCollection<Project>(projects);
            }
        }

        private void RefreshDataGrid()
        {
            LoadProjects();
            dgProject.ItemsSource = allproject;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProject addProject = new AddProject();
            addProject.Closed += (s, args) => RefreshDataGrid();
            addProject.Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
            {
                EditProject editProject = new EditProject(SelectedProject);
                editProject.Closed += (s, args) => RefreshDataGrid();
                editProject.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un projet à modifier.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.allproject != null && SelectedProject != null)
            {
                using (MakeoProjectContext context = new MakeoProjectContext())
                {
                    // Load the project with related entities
                    var project = context.Projects
                        .Include(p => p.ProjetCompetences)
                        .Include(p => p.FreelanceProjects)
                        .FirstOrDefault(p => p.Id == SelectedProject.Id);

                    if (project != null)
                    {
                        // Remove related entities
                        context.ProjetCompetences.RemoveRange(project.ProjetCompetences);
                        context.FreelanceProjects.RemoveRange(project.FreelanceProjects);

                        // Remove the project
                        context.Projects.Remove(project);
                        context.SaveChanges();

                        // Update the ObservableCollection
                        this.allproject.Remove(SelectedProject);
                        RefreshDataGrid();
                    }
                }
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgProject.SelectedItem is Project selectedProject)
            {
                var infoWindow = new InfoProject(selectedProject);
                infoWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance pour voir les détails.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
