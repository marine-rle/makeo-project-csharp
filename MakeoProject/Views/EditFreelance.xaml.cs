using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
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
        public Freelance SelectedFreelance { get; set; }
        public ObservableCollection<Competence> AvailableCompetences { get; set; }
        public ObservableCollection<Competence> SelectedCompetences { get; set; }

        public EditFreelance(Freelance selectedFreelance)
        {
            InitializeComponent();
            SelectedFreelance = selectedFreelance;
            DataContext = this;
            LoadCompetences();
            FillFormWithData();
        }

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
                ErrorMessage.Text = "Vous avez dépassé la limite de 50 caractères.";
            }
            else if (length < 1)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Le prénom doit contenir au moins 1 caractère.";
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
                ErrorMessageSurname.Text = "Vous avez dépassé la limite de 50 caractères.";
            }
            else if (length < 1)
            {
                ErrorMessageSurname.Visibility = Visibility.Visible;
                ErrorMessageSurname.Text = "Le nom doit contenir au moins 1 caractère.";
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
                ErrorMessageDescription.Text = "Vous avez dépassé la limite de 255 caractères.";
            }
            else if (length < 10)
            {
                ErrorMessageDescription.Visibility = Visibility.Visible;
                ErrorMessageDescription.Text = "La description doit contenir au moins 10 caractères.";
            }
            else
            {
                ErrorMessageDescription.Visibility = Visibility.Collapsed;
            }
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Username.Text.Length;
            CharacterCountUsername.Text = $"{length}/30 caractères";

            if (length > 30)
            {
                ErrorMessageUsername.Visibility = Visibility.Visible;
                ErrorMessageUsername.Text = "Vous avez dépassé la limite de 30 caractères.";
            }
            else if (length < 5)
            {
                ErrorMessageUsername.Visibility = Visibility.Visible;
                ErrorMessageUsername.Text = "L'identifiant doit contenir au moins 5 caractères.";
            }
            else
            {
                ErrorMessageUsername.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            int length = PasswordBox.Password.Length;
            CharacterCountPassword.Text = $"{length}/64 caractères";

            if (length > 64)
            {
                ErrorMessagePassword.Visibility = Visibility.Visible;
                ErrorMessagePassword.Text = "Vous avez dépassé la limite de 64 caractères.";
            }
            else if (length < 8)
            {
                ErrorMessagePassword.Visibility = Visibility.Visible;
                ErrorMessagePassword.Text = "Le mot de passe doit contenir au moins 8 caractères.";
            }
            else if (!Regex.IsMatch(PasswordBox.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$"))
            {
                ErrorMessagePassword.Visibility = Visibility.Visible;
                ErrorMessagePassword.Text = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.";
            }
            else
            {
                ErrorMessagePassword.Visibility = Visibility.Collapsed;
            }
        }

        private void FillFormWithData()
        {
            Name.Text = SelectedFreelance.Name;
            Surname.Text = SelectedFreelance.Surname;
            Description.Text = SelectedFreelance.Description;
            Username.Text = SelectedFreelance.Username;

            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
        }


        // Validation method for name and surname
        private bool IsValidName(string name)
        {
            // Check if the name contains only letters (including accented letters) and spaces
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-ZÀ-ÖØ-öø-ÿ\s]+$") && name.Length >= 1 && name.Length <= 50;
        }

        // Validation method for description
        private bool IsValidDescription(string description)
        {
            // Check if the description contains at least one non-space character
            return !string.IsNullOrWhiteSpace(description) && description.Length >= 10 && description.Length <= 255;
        }

        // Validation method for username
        private bool IsValidUsername(string username)
        {
            // Check if the username has at least 5 characters
            return !string.IsNullOrWhiteSpace(username) && username.Length >= 5 && username.Length <= 30;
        }

        // Validation method for password
        private bool IsValidPassword(string password)
        {
            // Check if the password has at least 8 characters and at most 64 characters
            // And meets complexity requirements: at least one uppercase letter, one lowercase letter, one digit, and one special character
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8 && password.Length <= 64 &&
                   Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.]).{8,}$"
);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private void AddCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompetences = AvailableCompetencesListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedCompetences)
            {
                SelectedCompetences.Add(selected);
                AvailableCompetences.Remove(selected);
            }

            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
        }

        private void RemoveCompetenceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompetences = SelectedCompetencesListBox.SelectedItems.Cast<Competence>().ToList();
            foreach (var selected in selectedCompetences)
            {
                SelectedCompetences.Remove(selected);
                AvailableCompetences.Add(selected);
            }

            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string surname = Surname.Text;
            string description = Description.Text;
            string username = Username.Text;
            string password = PasswordBox.Password; // Assuming Password is a PasswordBox

            if (IsValidName(name) && IsValidName(surname) && IsValidDescription(description) && IsValidUsername(username))
            {
                if (SelectedCompetencesListBox.Items.Count > 0)
                {
                    SelectedFreelance.Name = name;
                    SelectedFreelance.Surname = surname;
                    SelectedFreelance.Description = description;
                    SelectedFreelance.Username = username;
                    SelectedFreelance.UpdatedAt = DateTime.Now;

                    // Conditionally hash the password only if it's not empty
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        SelectedFreelance.Password = HashPassword(password);
                    }

                    try
                    {
                        using (MakeoProjectContext context = new MakeoProjectContext())
                        {
                            var existingCompetences = context.FreelanceCompetences.Where(fc => fc.FreelanceId == SelectedFreelance.Id).ToList();
                            context.FreelanceCompetences.RemoveRange(existingCompetences);
                            context.SaveChanges();

                            foreach (var competence in SelectedCompetences)
                            {
                                SelectedFreelance.FreelanceCompetences.Add(new FreelanceCompetence { FreelanceId = SelectedFreelance.Id, IdCompetences = competence.Id });
                            }

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
                MessageBox.Show("Veuillez entrer des informations valides.");
            }
        }

    }
}
