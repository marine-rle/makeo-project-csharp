using MakeoProject.DbLib.Class;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace MakeoProject.Views
{
    public partial class InfoProject : Window
    {
        public ObservableCollection<Freelance> AssociatedFreelances { get; set; }



        private Project selectedProject;




        public InfoProject(Project project)
        {
            InitializeComponent();
            AssociatedFreelances = new ObservableCollection<Freelance>();

            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var freelances = context.FreelanceProjects
                                         .Where(fp => fp.ProjectId == project.Id)
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
            DataContext = selectedProject = project;

            bool isAdmin = Session.CurrentUser != null && Session.CurrentUser.Admin;

            // Afficher ou masquer les boutons en fonction du statut de l'administrateur
            AddButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTache addTache = new AddTache(selectedProject); // Passer selectedProject à AddTache
            addTache.Show();
        }
    }
}
