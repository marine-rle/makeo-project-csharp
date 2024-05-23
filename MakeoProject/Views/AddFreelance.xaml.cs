using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MakeoProject.Views
{
    /// <summary>
    /// Interaction logic for adding a new freelance.
    /// </summary>
    public partial class AddFreelance : Window
    {
        // Collection of available competences
        public ObservableCollection<Competence> AvailableCompetences { get; set; }

        // Collection of selected competences
        public ObservableCollection<Competence> SelectedCompetences { get; set; }

        public AddFreelance()
        {
            InitializeComponent();
            DataContext = this;
            LoadCompetences();

            // Set item source for list boxes
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
            SelectedCompetences = new ObservableCollection<Competence>();
            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
        }


        // Method to load competences from the database
        private void LoadCompetences()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                AvailableCompetences = new ObservableCollection<Competence>(context.Competences.ToList());
            }
        }

        // Event handler for adding competence button click
        private void AddCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selected in AvailableCompetencesListBox.SelectedItems.Cast<Competence>().ToList())
            {
                if (!SelectedCompetences.Contains(selected))
                {
                    SelectedCompetences.Add(selected);
                    AvailableCompetences.Remove(selected);
                }
            }
        }

        // Event handler for removing competence button click
        private void RemoveCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selected in SelectedCompetencesListBox.SelectedItems.Cast<Competence>().ToList())
            {
                if (!AvailableCompetences.Contains(selected))
                {
                    AvailableCompetences.Add(selected);
                    SelectedCompetences.Remove(selected);
                }
            }
        }

        // Event handler for confirm button click
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string surname = Surname.Text;
            string description = Description.Text;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(description))
            {
                // Check if there are selected competences
                if (SelectedCompetencesListBox.Items.Count > 0)
                {
                    Freelance newFreelance = new Freelance
                    {
                        Name = name,
                        Surname = surname,
                        Description = description,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    // Add selected competences to the new freelance
                    foreach (var competence in SelectedCompetences)
                    {
                        newFreelance.FreelanceCompetences.Add(new FreelanceCompetence { IdCompetences = competence.Id });
                    }

                    try
                    {
                        using (MakeoProjectContext context = new MakeoProjectContext())
                        {
                            // Add the new freelance to the database
                            context.Freelances.Add(newFreelance);
                            context.SaveChanges();
                            MessageBox.Show("Freelance ajouté avec succès.");
                            this.Close();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show("Une erreur est survenue lors de la modification du client : " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Une erreur est survenue : " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner au moins une compétence.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs");
            }
        }

    }
}
