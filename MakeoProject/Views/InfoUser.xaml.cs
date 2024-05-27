using MakeoProject.DbLib.Class;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MakeoProject.Views
{
    public partial class InfoUser : Window
    {
        public ObservableCollection<Project> AssociatedProjects { get; set; }

        public InfoUser(User selectedUser)
        {
            InitializeComponent();

            AssociatedProjects = new ObservableCollection<Project>();

            // Récupérez les projets associés à l'utilisateur sélectionné à partir de la base de données
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var projects = context.Projects
                                      .Where(p => p.UserId == selectedUser.Id && p.Statut.Name != "Brouillon")
                                      .ToList();

                foreach (var project in projects)
                {
                    AssociatedProjects.Add(project);
                }
            }

            // Liez la collection de projets à un contrôle dans votre interface utilisateur
            projectListBox.ItemsSource = AssociatedProjects;

            // Définissez le DataContext de la fenêtre sur l'utilisateur sélectionné pour afficher ses autres détails
            DataContext = selectedUser;
        }
    }
}
