using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MakeoProject.Views
{
    /// <summary>
    /// Logique d'interaction pour Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        // Event handler for name text changed
        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = NameTextBox.Text.Length;
            CharacterCountName.Text = $"{length}/50 caractères";

            if (length > 50)
            {
                ErrorMessageName.Visibility = Visibility.Visible;
                ErrorMessageName.Text = "Vous avez dépassé la limite de 50 caractères.";
            }
            else if (length < 1)
            {
                ErrorMessageName.Visibility = Visibility.Visible;
                ErrorMessageName.Text = "Le prénom doit contenir au moins 1 caractère.";
            }
            else
            {
                ErrorMessageName.Visibility = Visibility.Collapsed;
            }
        }

        // Event handler for surname text changed
        private void Surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = SurnameTextBox.Text.Length;
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

        // Event handler for description text changed
        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = DescriptionTextBox.Text.Length;
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

        // Event handler for username text changed
        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = UsernameTextBox.Text.Length;
            CharacterCountUsername.Text = $"{length}/30 caractères";

            if (length > 30)
            {
                ErrorMessageUsername.Visibility = Visibility.Visible;
                ErrorMessageUsername.Text = "Vous avez dépassé la limite de 30 caractères.";
            }
            else if (length < 5)
            {
                ErrorMessageUsername.Visibility = Visibility.Visible;
                ErrorMessageUsername.Text = "Le nom d'utilisateur doit contenir au moins 5 caractères.";
            }
            else
            {
                ErrorMessageUsername.Visibility = Visibility.Collapsed;
            }
        }

        // Event handler for password text changed
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

        private bool IsValidPassword(string password)
        {
            // Check if the password has at least PasswordMinLength characters and at most PasswordMaxLength characters
            // And meets complexity requirements: at least one uppercase letter, one lowercase letter, one digit, and one special character
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8 && password.Length <= 64 &&
                   Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.]).{8,}$"
);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string description = DescriptionTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            List<string> errorMessages = new List<string>();

            if (!IsValidName(name))
                errorMessages.Add("Le prénom n'est pas valide.");
            if (!IsValidName(surname))
                errorMessages.Add("Le nom n'est pas valide.");
            if (!IsValidDescription(description))
                errorMessages.Add("La description n'est pas valide.");
            if (!IsValidUsername(username))
                errorMessages.Add("Le nom d'utilisateur n'est pas valide.");
            if (!IsValidPassword(password))
                errorMessages.Add("Le mot de passe n'est pas valide.");

            if (errorMessages.Any())
            {
                string errorMessage = string.Join("\n", errorMessages);
                MessageBox.Show(errorMessage, "Erreurs de validation");
            }
            else
            {
                try
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        var newFreelance = new Freelance
                        {
                            Name = name,
                            Surname = surname,
                            Description = description,
                            Username = username,
                            Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        context.Freelances.Add(newFreelance);
                        context.SaveChanges();
                        MessageBox.Show("Freelance enregistré avec succès.");
                        this.Close();
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Une erreur est survenue lors de l'ajout du freelance : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                }
            }
        }

    }
}
