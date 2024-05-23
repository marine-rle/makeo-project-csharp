using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
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
    /// Logique d'interaction pour Freelances.xaml
    /// </summary>
    public partial class Freelances : UserControl
    {

        public ObservableCollection<Freelance> allfreelance { get; set; }

        public Freelance? SelectedFreelance { get; set; }

        public Freelances()
        {
            InitializeComponent();

            this.DataContext = this;

            using (MakeoProjectContext allfreelances = new())
            {
                allfreelance = new ObservableCollection<Freelance>(allfreelances.Freelances.Include(p => p.FreelanceCompetences).ThenInclude(pc => pc.IdCompetencesNavigation).ToList());
            }

     

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
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;
                dataColumn.Binding = new Binding(column.Key);
                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(30);
                        break;
                    case "Name":
                        dataColumn.Width = new DataGridLength(50);
                        break;
                    case "Surname":
                        dataColumn.Width = new DataGridLength(50);
                        break;
                    case "Description":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        dataColumn.Binding = new Binding("Skills");
                        break;
                }
                dgFreelances.Columns.Add(dataColumn);
            }

            dgFreelances.AutoGenerateColumns = false;
        }
    }
}
