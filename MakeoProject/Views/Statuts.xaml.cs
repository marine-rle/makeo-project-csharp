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
    /// Logique d'interaction pour Statuts.xaml
    /// </summary>
    public partial class Statuts : UserControl
    {
        public ObservableCollection<Statut> allstatut { get; set; }
        public Statut? SelectedStatut { get; set; }

        public Statuts()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadStatuts();

            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "Nom" }
            };

            foreach (var column in columnsToDisplay)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;
                dataColumn.Binding = new Binding(column.Key);
                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(30);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        break;
                }
                dgStatuts.Columns.Add(dataColumn);
            }

            dgStatuts.AutoGenerateColumns = false;
        }

        private void LoadStatuts()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                allstatut = new ObservableCollection<Statut>(context.Statuts.ToList());
            }
            dgStatuts.ItemsSource = allstatut;
        }

        private void RefreshDataGrid()
        {
            LoadStatuts();
            dgStatuts.ItemsSource = allstatut;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddStatuts addStatuts = new AddStatuts();
            addStatuts.Closed += (s, args) => RefreshDataGrid();
            addStatuts.Show();
        }
    }
}
