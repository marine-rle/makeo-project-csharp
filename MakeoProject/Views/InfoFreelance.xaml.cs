using MakeoProject.DbLib.Class;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MakeoProject.Views
{
    public partial class InfoFreelance : Window
    {
        public ObservableCollection<Project> AssociatedProjects { get; set; }

        public InfoFreelance(Freelance selectedFreelance)
        {
            InitializeComponent();

            AssociatedProjects = new ObservableCollection<Project>();

            // Récupérez les projets associés au freelance sélectionné à partir de la base de données
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var projects = context.FreelanceProjects
                                     .Where(fp => fp.FreelanceId == selectedFreelance.Id)
                                     .Select(fp => fp.Project)
                                     .ToList();

                foreach (var project in projects)
                {
                    AssociatedProjects.Add(project);
                }
            }

            // Liez la collection de projets à un contrôle dans votre interface utilisateur
            // Par exemple, si vous utilisez une ListBox nommée 'projectListBox':
            projectListBox.ItemsSource = AssociatedProjects;

            // Définissez le DataContext de la fenêtre sur le freelance sélectionné pour afficher ses autres détails
            DataContext = selectedFreelance;
        }
    }
}
