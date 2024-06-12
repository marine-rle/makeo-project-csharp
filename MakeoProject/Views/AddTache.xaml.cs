using MakeoProject.DbLib.Class;
using System;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MakeoProject.Views
{
    public partial class AddTache : Window
    {
        private readonly Project _selectedProject;

        public AddTache(Project selectedProject)
        {
            InitializeComponent();
            _selectedProject = selectedProject;
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des champs
            string titre = Titre.Text;
            string description = Description.Text;
            DateTime date = Date.SelectedDate ?? DateTime.Now; // Utilisez la date sélectionnée ou la date actuelle
            int duree = Convert.ToInt32(Duree.Text);

            // Créer une nouvelle tâche
            Taches nouvelleTache = new Taches
            {
                ProjectId = _selectedProject.Id,
                Title = titre,
                Description = description,
                Date = date,
                Duree = duree,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Ajouter la nouvelle tâche à la base de données
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                context.Taches.Add(nouvelleTache);
                context.SaveChanges();
            }

            // Fermer la fenêtre d'ajout de tâche
            Close();
        }
    }
}
