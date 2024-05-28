using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MakeoProject.Views
{
    public partial class EditFreelance : Window
    {
        // Property to store the selected freelance data
        public Freelance SelectedFreelance { get; set; }

        // Collection of available competences
        public ObservableCollection<Competence> AvailableCompetences { get; set; }

        // Collection of selected competences
        public ObservableCollection<Competence> SelectedCompetences { get; set; }

        // Constructor accepting the selected freelance data
        public EditFreelance(Freelance selectedFreelance)
        {
            InitializeComponent();
            SelectedFreelance = selectedFreelance;
            DataContext = this;
            LoadCompetences();
            FillFormWithData();
        }

        // Method to load competences from the database
        private void LoadCompetences()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var allCompetences = context.Competences.ToList();
                var selectedCompetencesIds = SelectedFreelance.FreelanceCompetences.Select(fc => fc.IdCompetences).ToList();
                AvailableCompetences = new ObservableCollection<Competence>(allCompetences.Where(c => !selectedCompetencesIds.Contains(c.Id)));
                SelectedCompetences = new ObservableCollection<Competence>(allCompetences.Where(c => selectedCompetencesIds.Contains(c.Id)));
            }
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Name.Text.Length;
            CharacterCount.Text = $"{length}/50 caractères";

            if (length > 50)
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void Surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Surname.Text.Length;
            CharacterCountSurname.Text = $"{length}/50 caractères";

            if (length > 50)
            {
                ErrorMessageSurname.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessageSurname.Visibility = Visibility.Collapsed;
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

        // Method to fill the form with data of the selected freelance
        private void FillFormWithData()
        {
            Name.Text = SelectedFreelance.Name;
            Surname.Text = SelectedFreelance.Surname;
            Description.Text = SelectedFreelance.Description;

            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
        }

        // Validation method for name and surname
        private bool IsValidName(string name)
        {
            // Check if the name contains only letters (including accented letters) and spaces
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-ZÀ-ÖØ-öø-ÿ\s]+$") && name.Length <= 50;
        }


        // Validation method for description
        private bool IsValidDescription(string description)
        {
            // Check if the description contains at least one non-space character
            return !string.IsNullOrWhiteSpace(description) && description.Length <= 255;
        }

        // Event handler for adding a competence
        private void AddCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompetences = AvailableCompetencesListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedCompetences)
            {
                SelectedCompetences.Add(selected);
                AvailableCompetences.Remove(selected);
            }

            // Update the ListBox items sources
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
        }

        // Event handler for removing a competence
        private void RemoveCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompetences = SelectedCompetencesListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedCompetences)
            {
                SelectedCompetences.Remove(selected);
                AvailableCompetences.Add(selected);
            }

            // Update the ListBox items sources
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
        }

        // Event handler for confirming changes
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string surname = Surname.Text;
            string description = Description.Text;

            if (IsValidName(name) && IsValidName(surname) && IsValidDescription(description))
            {
                if (SelectedCompetencesListBox.Items.Count > 0)
                {
                    // Update the freelance data with the modified values
                    SelectedFreelance.Name = name;
                    SelectedFreelance.Surname = surname;
                    SelectedFreelance.Description = description;
                    SelectedFreelance.UpdatedAt = DateTime.Now;

                    try
                    {
                        using (MakeoProjectContext context = new MakeoProjectContext())
                        {
                            // Remove existing competences
                            var existingCompetences = context.FreelanceCompetences.Where(fc => fc.FreelanceId == SelectedFreelance.Id).ToList();
                            context.FreelanceCompetences.RemoveRange(existingCompetences);
                            context.SaveChanges();

                            // Add the new competences
                            foreach (var competence in SelectedCompetences)
                            {
                                SelectedFreelance.FreelanceCompetences.Add(new FreelanceCompetence { FreelanceId = SelectedFreelance.Id, IdCompetences = competence.Id });
                            }

                            // Update the freelance in the database
                            context.Update(SelectedFreelance);
                            context.SaveChanges();

                            MessageBox.Show("Modifications enregistrées avec succès.");
                            this.Close();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show("Une erreur est survenue lors de la modification du freelance : " + ex.Message);
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
                MessageBox.Show("Veuillez entrer un nom et un prénom valides (contenant des lettres) et remplir la description.");
            }
        }
    }
}
