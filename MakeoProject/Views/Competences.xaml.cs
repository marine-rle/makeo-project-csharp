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
    /// <summary>
    /// Logique d'interaction pour Competences.xaml
    /// </summary>
    public partial class Competences : UserControl
    {
        public ObservableCollection<Competence> allcompetence { get; set; }
        public Competence? SelectedCompetence { get; set; }

        public Competences()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadCompetences();

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

        private void LoadCompetences()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                allcompetence = new ObservableCollection<Competence>(context.Competences.ToList());
            }
            dgCompetences.ItemsSource = allcompetence;
        }

        private void RefreshDataGrid()
        {
            LoadCompetences();
            dgCompetences.ItemsSource = allcompetence;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCompetences addCompetences = new AddCompetences();
            addCompetences.Closed += (s, args) => RefreshDataGrid();
            addCompetences.Show();
        }
    }
}
