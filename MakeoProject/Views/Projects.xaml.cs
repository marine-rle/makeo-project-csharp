using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Logique d'interaction pour Projects.xaml
    /// </summary>
    public partial class Projects : UserControl
    {

        public ObservableCollection<Project> allproject { get; set; }

        public Project? SelectedProject { get; set; }

        public Projects()
        {
            InitializeComponent();

            this.DataContext = this;

            using (MakeoProjectContext allprojects = new MakeoProjectContext())
            {
                allproject = new ObservableCollection<Project>(allprojects.Projects
                                                                        .Include(p => p.User)
                                                                        .Include(p => p.Statut)
                                                                        .Include(p => p.ProjetCompetences)
                                                                            .ThenInclude(pc => pc.IdCompetencesNavigation)
                                                                        .Where(p => p.Statut.Name != "Brouillon")
                                                                        .ToList());
            }




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
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;
                dataColumn.Binding = new Binding(column.Key);
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

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.allproject != null && SelectedProject != null)
            {
                using (MakeoProjectContext context = new())
                {
                    context.Projects.Remove(SelectedProject);
                    context.SaveChanges();
                    this.allproject.Remove(SelectedProject);
                }
            }
        }
    }
    
}
