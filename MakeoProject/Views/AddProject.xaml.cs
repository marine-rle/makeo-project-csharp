using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace MakeoProject.Views
{
    public partial class AddProject : Window
    {
        public Project NewProject { get; set; } = new Project();
        public ObservableCollection<Competence> AvailableSkills { get; set; } = new ObservableCollection<Competence>();
        public ObservableCollection<Competence> SelectedSkills { get; set; } = new ObservableCollection<Competence>();
        public ObservableCollection<Freelance> AvailableFreelances { get; set; } = new ObservableCollection<Freelance>();
        public ObservableCollection<Freelance> SelectedFreelances { get; set; } = new ObservableCollection<Freelance>();
        public ObservableCollection<User> Clients { get; set; } = new ObservableCollection<User>();

        public ObservableCollection<Statut> Statuses { get; set; } = new ObservableCollection<Statut>();


        public AddProject()
        {
            InitializeComponent();
            DataContext = this;
            LoadStatuses();
            LoadSkills();
            LoadFreelances();
            LoadClients();
        }

       private void LoadStatuses()
       {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allStatuses = context.Statuts.Where(s => s.Id != 1).ToList();
                Statuses = new ObservableCollection<Statut>(allStatuses);
            }
       }


        private void LoadSkills()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allSkills = context.Competences.ToList();
                AvailableSkills = new ObservableCollection<Competence>(allSkills);
            }
        }

        private void LoadFreelances()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allFreelances = context.Freelances.Include(f => f.FreelanceCompetences).ThenInclude(fc => fc.IdCompetencesNavigation).ToList();
                AvailableFreelances = new ObservableCollection<Freelance>(allFreelances);
            }
        }



        private void LoadClients()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allClients = context.Users.ToList();
                Clients = new ObservableCollection<User>(allClients);
            }
        }


        private bool IsValidTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        private bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
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
            if (StatusComboBox.SelectedIndex == -1 || StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un statut.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ulong statusId = (ulong)StatusComboBox.SelectedValue;

            if (IsValidTitle(title) && IsValidDescription(description) && Users.SelectedItem != null)
            {
                if (SelectedSkills.Count == 0)
                {
                    MessageBox.Show("Veuillez sélectionner au moins une compétence.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ulong userId = (ulong)Users.SelectedValue;
                DateTime now = DateTime.Now;
                DateTime formattedDateDemande = DateTime.ParseExact(now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);


                NewProject.Title = title;
                NewProject.Description = description;
                NewProject.DateDemande = formattedDateDemande;
                NewProject.CreatedAt = now;
                NewProject.UpdatedAt = now;
                NewProject.UserId = userId;
                NewProject.StatutId = statusId;

                try
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        foreach (var skill in SelectedSkills)
                        {
                            NewProject.ProjetCompetences.Add(new ProjetCompetence { IdCompetences = skill.Id });
                        }

                        foreach (var freelance in SelectedFreelances)
                        {
                            NewProject.FreelanceProjects.Add(new FreelanceProject { FreelanceId = freelance.Id });
                        }

                        context.Projects.Add(NewProject);
                        context.SaveChanges();

                        MessageBox.Show("Le projet a été ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite lors de l'ajout du projet : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



    }
}
