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
            string username = Username.Text;
            string password = PasswordBox.Password;

            if (IsValidName(name) && IsValidName(surname) && IsValidDescription(description) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // Check if there are selected competences
                if (SelectedCompetencesListBox.Items.Count > 0)
                {
                    Freelance newFreelance = new Freelance
                    {
                        Name = name,
                        Surname = surname,
                        Description = description,
                        Username = username,
                        Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password
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
                        MessageBox.Show("Une erreur est survenue lors de l'ajout d'un freelance : " + ex.Message);
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
                MessageBox.Show("Veuillez remplir tous les champs requis.");
            }
        }
    }
}
