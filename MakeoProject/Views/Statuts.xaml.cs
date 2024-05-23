using MakeoProject.DbLib.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            using (MakeoProjectContext allstatuts = new())
            {
                allstatut = new ObservableCollection<Statut>(allstatuts.Statuts.ToList());
            }

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
    }
}
