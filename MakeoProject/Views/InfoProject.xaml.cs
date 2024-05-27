using MakeoProject.DbLib.Class;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace MakeoProject.Views
{
    public partial class InfoProject : Window
    {
        public ObservableCollection<Freelance> AssociatedFreelances { get; set; }

        public InfoProject(Project selectedProject)
        {
            InitializeComponent();
            AssociatedFreelances = new ObservableCollection<Freelance>();

            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var freelances = context.FreelanceProjects
                                         .Where(fp => fp.ProjectId == selectedProject.Id)
                                         .Select(fp => fp.Freelance)
                                         .ToList();

                foreach (var freelance in freelances)
                {
                    AssociatedFreelances.Add(freelance);
                }
            }

            // Liez la collection de freelances à un contrôle dans votre interface utilisateur
            // Par exemple, si vous utilisez une ListBox nommée 'freelanceListBox':
            freelanceListBox.ItemsSource = AssociatedFreelances;

            // Définissez le DataContext de la fenêtre sur le projet sélectionné pour afficher ses autres détails
            DataContext = selectedProject;
        }
    }
}
