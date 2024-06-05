using MakeoProject.DbLib.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MakeoProject.Views
{
    public partial class Competences : UserControl
    {
        public ObservableCollection<Competence> allcompetence { get; set; }
        public Competence? SelectedCompetence { get; set; }

        public Competences()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadCompetences();
            InitializeDataGridColumns();

            // Vérifier le statut de l'utilisateur actuellement connecté
            bool isAdmin = Session.CurrentUser != null && Session.CurrentUser.Admin;

            // Afficher ou masquer les boutons Ajouter, Modifier et Supprimer en fonction du statut admin
            AddButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            EditButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            DeleteButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        // Méthode pour charger les compétences depuis la base de données
        private void LoadCompetences()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                allcompetence = new ObservableCollection<Competence>(context.Competences.ToList());
            }
            dgCompetences.ItemsSource = allcompetence;
        }

        // Méthode pour actualiser le DataGrid après des modifications
        private void RefreshDataGrid()
        {
            LoadCompetences();
            dgCompetences.ItemsSource = allcompetence;
        }

        // Méthode pour initialiser les colonnes du DataGrid
        private void InitializeDataGridColumns()
        {
            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "Nom" },
                { "CreatedAt", "Créé le" },
                { "UpdatedAt", "Mis à jour le" }
            };

            foreach (var column in columnsToDisplay)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;

                if (column.Key == "CreatedAt" || column.Key == "UpdatedAt")
                {
                    dataColumn.Binding = new Binding(column.Key) { Converter = new DateTimeFormatConverter() };
                }
                else
                {
                    dataColumn.Binding = new Binding(column.Key);
                }

                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(30);
                        break;
                    case "CreatedAt":
                    case "UpdatedAt":
                        dataColumn.Width = new DataGridLength(200);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        break;
                }
                dgCompetences.Columns.Add(dataColumn);
            }

            dgCompetences.AutoGenerateColumns = false;
        }

        // Gestionnaire d'événement pour le clic sur le bouton Ajouter
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCompetences addCompetences = new AddCompetences();
            addCompetences.Closed += (s, args) => RefreshDataGrid();
            addCompetences.Show();
        }

        // Gestionnaire d'événement pour le clic sur le bouton Modifier
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCompetence != null)
            {
                EditCompetences editCompetences = new EditCompetences(SelectedCompetence);
                editCompetences.Closed += (s, args) => RefreshDataGrid();
                editCompetences.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une compétence à modifier.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Gestionnaire d'événement pour le clic sur le bouton Supprimer
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCompetence != null)
            {
                using (MakeoProjectContext context = new MakeoProjectContext())
                {
                    bool isUsedInProject = context.ProjetCompetences.Any(pc => pc.IdCompetences == SelectedCompetence.Id);
                    bool isUsedInFreelance = context.FreelanceCompetences.Any(fc => fc.IdCompetences == SelectedCompetence.Id);

                    if (isUsedInProject || isUsedInFreelance)
                    {
                        MessageBox.Show("Cette compétence est utilisée dans un ou plusieurs projets ou par des freelances et ne peut pas être supprimée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        var competenceToRemove = context.Competences.FirstOrDefault(c => c.Id == SelectedCompetence.Id);
                        if (competenceToRemove != null)
                        {
                            context.Competences.Remove(competenceToRemove);
                            context.SaveChanges();
                            RefreshDataGrid();
                            MessageBox.Show("La compétence a été supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une compétence à supprimer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
