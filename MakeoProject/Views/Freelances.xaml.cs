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
    public partial class Freelances : UserControl
    {
        public ObservableCollection<Freelance> allfreelance { get; set; }
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

        public Freelances()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadFreelances();

            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "Prénom" },
                { "Surname", "Nom" },
                { "Description", "Description" },
                { "Competence", "Compétences" },
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
                        dataColumn.Width = new DataGridLength(50);
                        break;
                    case "Name":
                    case "Surname":
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                        dataColumn.MinWidth = 150;
                        break;
                    case "Description":
                        dataColumn.Width = new DataGridLength(200);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        dataColumn.Binding = new Binding("Skills");
                        break;
                }
                dgFreelances.Columns.Add(dataColumn);
            }

            dgFreelances.AutoGenerateColumns = false;

            // Masquer le bouton "Ajouter" et "Supprimer" si l'utilisateur actuel n'est pas administrateur
            if (Session.CurrentUser != null && !Session.CurrentUser.Admin)
            {
                AddButton.Visibility = Visibility.Collapsed;
                RemoveButton.Visibility = Visibility.Collapsed;
            }
        }

        private void RefreshDataGrid()
        {
            LoadFreelances();
            dgFreelances.ItemsSource = allfreelance;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez si l'utilisateur actuel est administrateur avant d'afficher la fenêtre d'ajout
            if (Session.CurrentUser != null && Session.CurrentUser.Admin)
            {
                AddFreelance addFreelance = new AddFreelance();
                addFreelance.Closed += (s, args) => RefreshDataGrid();
                addFreelance.Show();
            }
            else
            {
                MessageBox.Show("Vous n'êtes pas autorisé à ajouter un nouveau freelance.", "Accès non autorisé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFreelance != null)
            {
                if (Session.CurrentUser != null && Session.CurrentUser.Admin)
                {
                    MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce freelance ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (MakeoProjectContext context = new())
                        {
                            var freelanceProjects = context.FreelanceProjects.Where(fp => fp.FreelanceId == SelectedFreelance.Id).ToList();
                            context.FreelanceProjects.RemoveRange(freelanceProjects);
                            context.SaveChanges();

                            context.Freelances.Remove(SelectedFreelance);
                            context.SaveChanges();

                            allfreelance.Remove(SelectedFreelance);
                            RefreshDataGrid();

                            MessageBox.Show("Le freelance a bien été supprimé.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vous n'êtes pas autorisé à supprimer ce freelance.", "Accès non autorisé", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance à supprimer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFreelance != null)
            {
                if (Session.CurrentUser != null && Session.CurrentUser.Id == SelectedFreelance.Id)
                {
                    EditFreelance editFreelance = new EditFreelance(SelectedFreelance);
                    editFreelance.Closed += (s, args) => RefreshDataGrid();
                    editFreelance.Show();
                }
                else
                {
                    MessageBox.Show("Vous n'êtes pas autorisé à modifier les données d'un autre freelance.", "Accès non autorisé", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un freelance à modifier.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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

        private void dgFreelances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFreelances.SelectedItem is Freelance selectedFreelance)
            {
                SelectedFreelance = selectedFreelance;
            }
        }
    }
}
