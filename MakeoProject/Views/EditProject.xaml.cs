using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MakeoProject.Views
{
    public partial class EditProject : Window
    {
        public Project SelectedProject { get; set; }
        public ObservableCollection<Competence> AvailableSkills { get; set; } = new ObservableCollection<Competence>();
        public ObservableCollection<Competence> SelectedSkills { get; set; } = new ObservableCollection<Competence>();
        public ObservableCollection<Freelance> AvailableFreelances { get; set; } = new ObservableCollection<Freelance>();
        public ObservableCollection<Freelance> SelectedFreelances { get; set; } = new ObservableCollection<Freelance>();

        public ObservableCollection<Statut> Statuses { get; set; } = new ObservableCollection<Statut>();
        public ulong OriginalStatusId { get; set; }

        public EditProject(Project selectedProject)
        {
            InitializeComponent();
            SelectedProject = selectedProject;
            DataContext = this;
            LoadStatuses();
            LoadSkills();
            LoadFreelances();
            FillFormWithData();
        }


        private void LoadStatuses()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allStatuses = context.Statuts.ToList();
                var filteredStatuses = allStatuses.Where(s => s.Name != "Brouillon").ToList();
                Statuses = new ObservableCollection<Statut>(filteredStatuses);
            }
        }


        private void LoadSkills()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allSkills = context.Competences.ToList();
                var selectedSkillsIds = SelectedProject.ProjetCompetences.Select(pc => pc.IdCompetences).ToList();
                AvailableSkills = new ObservableCollection<Competence>(allSkills.Where(c => !selectedSkillsIds.Contains(c.Id)));
                SelectedSkills = new ObservableCollection<Competence>(allSkills.Where(c => selectedSkillsIds.Contains(c.Id)));
            }
        }

        private void LoadFreelances()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allFreelances = context.Freelances.Include(f => f.FreelanceCompetences).ThenInclude(fc => fc.IdCompetencesNavigation).ToList();
                var selectedFreelances = context.FreelanceProjects
                    .Where(fp => fp.ProjectId == SelectedProject.Id)
                    .Include(fp => fp.Freelance)
                    .Select(fp => fp.Freelance)
                    .ToList();

                var selectedFreelancesIds = selectedFreelances.Select(f => f.Id).ToList();

                AvailableFreelances = new ObservableCollection<Freelance>(allFreelances.Where(f => !selectedFreelancesIds.Contains(f.Id)));
                SelectedFreelances = new ObservableCollection<Freelance>(selectedFreelances);
            }
        }

        private void FillFormWithData()
        {
            Title.Text = SelectedProject.Title;
            Description.Text = SelectedProject.Description;

            SelectedSkillsListBox.ItemsSource = SelectedSkills;
            AvailableSkillsListBox.ItemsSource = AvailableSkills;

            AvailableFreelancesListBox.ItemsSource = AvailableFreelances;
            SelectedFreelancesListBox.ItemsSource = SelectedFreelances;

            StatusComboBox.ItemsSource = Statuses;
            StatusComboBox.SelectedValue = SelectedProject.StatutId;

            OriginalStatusId = SelectedProject.StatutId;
        }

        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Title.Text.Length;
            CharacterCount.Text = $"{length}/100 caractères";

            if (length > 100)
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Description.Text.Length;
            CharacterCountDescription.Text = $"{length}/255 caractères";

            if (length > 255)
            {
                ErrorMessageDescription.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessageDescription.Visibility = Visibility.Collapsed;
            }
        }

       private bool IsValidTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title) && title.Length <= 100;
        }

        private bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description) && description.Length <= 255;
        }

        private void AddSkillButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSkills = AvailableSkillsListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedSkills)
            {
                SelectedSkills.Add(selected);
                AvailableSkills.Remove(selected);
            }

            AvailableSkillsListBox.ItemsSource = AvailableSkills;
            SelectedSkillsListBox.ItemsSource = SelectedSkills;
        }

        private void RemoveSkillButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSkills = SelectedSkillsListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedSkills)
            {
                SelectedSkills.Remove(selected);
                AvailableSkills.Add(selected);
            }

            AvailableSkillsListBox.ItemsSource = AvailableSkills;
            SelectedSkillsListBox.ItemsSource = SelectedSkills;
        }

        private void AddFreelanceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFreelances = AvailableFreelancesListBox.SelectedItems.Cast<Freelance>().ToList();
            foreach (var selected in selectedFreelances)
            {
                SelectedFreelances.Add(selected);
                AvailableFreelances.Remove(selected);
            }

            AvailableFreelancesListBox.ItemsSource = AvailableFreelances;
            SelectedFreelancesListBox.ItemsSource = SelectedFreelances;
        }

        private void RemoveFreelanceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFreelances = SelectedFreelancesListBox.SelectedItems.Cast<Freelance>().ToList();
            foreach (var selected in selectedFreelances)
            {
                SelectedFreelances.Remove(selected);
                AvailableFreelances.Add(selected);
            }

            AvailableFreelancesListBox.ItemsSource = AvailableFreelances;
            SelectedFreelancesListBox.ItemsSource = SelectedFreelances;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string title = Title.Text;
            string description = Description.Text;
            ulong statusId = (ulong)StatusComboBox.SelectedValue;

            if (IsValidTitle(title) && IsValidDescription(description) && statusId > 0)
            {
                SelectedProject.Title = title;
                SelectedProject.Description = description;
                SelectedProject.UpdatedAt = DateTime.Now;

                // Recherche du statut correspondant dans la liste
                Statut selectedStatus = Statuses.FirstOrDefault(s => s.Id == statusId);
                if (selectedStatus != null)
                {
                    SelectedProject.Statut = selectedStatus;
                }
                else
                {
                    MessageBox.Show("Statut invalide sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Mettre à jour les compétences du projet
                SelectedProject.ProjetCompetences.Clear();
                foreach (var skill in SelectedSkills)
                {
                    SelectedProject.ProjetCompetences.Add(new ProjetCompetence { ProjectId = SelectedProject.Id, IdCompetences = skill.Id });
                }

                // Mettre à jour les freelances associés au projet
                SelectedProject.FreelanceProjects.Clear();
                foreach (var freelance in SelectedFreelances)
                {
                    SelectedProject.FreelanceProjects.Add(new FreelanceProject { ProjectId = SelectedProject.Id, FreelanceId = freelance.Id });
                }

                try
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        // Charger le projet existant depuis la base de données
                        var projectToUpdate = context.Projects
                            .Include(p => p.ProjetCompetences)
                            .Include(p => p.FreelanceProjects)
                            .FirstOrDefault(p => p.Id == SelectedProject.Id);

                        if (projectToUpdate != null)
                        {
                            // Mettre à jour les propriétés du projet
                            projectToUpdate.Title = SelectedProject.Title;
                            projectToUpdate.Description = SelectedProject.Description;
                            projectToUpdate.UpdatedAt = DateTime.Now;
                            projectToUpdate.Statut = SelectedProject.Statut;

                            // Mettre à jour les compétences du projet
                            projectToUpdate.ProjetCompetences.Clear();
                            foreach (var skill in SelectedProject.ProjetCompetences)
                            {
                                projectToUpdate.ProjetCompetences.Add(skill);
                            }

                            // Mettre à jour les freelances associés au projet
                            projectToUpdate.FreelanceProjects.Clear();
                            foreach (var freelance in SelectedProject.FreelanceProjects)
                            {
                                projectToUpdate.FreelanceProjects.Add(freelance);
                            }

                            // Enregistrer les modifications dans la base de données
                            context.SaveChanges();

                            MessageBox.Show("Le projet a été mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Impossible de trouver le projet à mettre à jour.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite lors de la mise à jour du projet : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}