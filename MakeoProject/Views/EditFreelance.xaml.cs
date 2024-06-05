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

        private void FillFormWithData()
        {
            Name.Text = SelectedFreelance.Name;
            Surname.Text = SelectedFreelance.Surname;
            Description.Text = SelectedFreelance.Description;
            Username.Text = SelectedFreelance.Username;

            SelectedCompetencesListBox.ItemsSource = SelectedCompetences;
            AvailableCompetencesListBox.ItemsSource = AvailableCompetences;
        }

        private bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-ZÀ-ÖØ-öø-ÿ\s]+$") && name.Length <= 50;
        }

        private bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description) && description.Length <= 255;
        }

        private bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && username.Length <= 50;
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
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
            string password = Password.Password; // Assuming Password is a PasswordBox

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
