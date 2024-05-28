using MakeoProject.DbLib.Class;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace MakeoProject.Views
{
    public partial class EditUser : Window
    {
        public User SelectedUser { get; set; }

        public EditUser(User selectedUser)
        {
            InitializeComponent();
            SelectedUser = selectedUser;
            DataContext = this;
            FillFormWithData();
        }

        private void FillFormWithData()
        {
            Name.Text = SelectedUser.Name;
            Surname.Text = SelectedUser.Surname;
            Email.Text = SelectedUser.Email;
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

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Email.Text.Length;
            CharacterCountEmail.Text = $"{length}/100 caractères";

            if (length > 100)
            {
                ErrorMessageEmail.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessageEmail.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsValidName(string name)
        {
            // Check if the name contains only letters (including accented letters) and spaces
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-ZÀ-ÖØ-öø-ÿ\s]+$") && name.Length <= 50;
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Length <= 100;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string surname = Surname.Text;
            string email = Email.Text;

            if (!IsValidName(name))
            {
                MessageBox.Show("Le prénom n'est pas valide. Il doit contenir uniquement des lettres et avoir au maximum 50 caractères.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidName(surname))
            {
                MessageBox.Show("Le nom n'est pas valide. Il doit contenir uniquement des lettres et avoir au maximum 50 caractères.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("L'adresse email n'est pas valide, il doit contenir un @ et au maximum 50 caractères.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedUser.Name = name;
            SelectedUser.Surname = surname;
            SelectedUser.Email = email;
            SelectedUser.UpdatedAt = DateTime.Now;

            try
            {
                using (MakeoProjectContext context = new MakeoProjectContext())
                {
                    var userToUpdate = context.Users.Find(SelectedUser.Id);
                    if (userToUpdate != null)
                    {
                        userToUpdate.Name = SelectedUser.Name;
                        userToUpdate.Surname = SelectedUser.Surname;
                        userToUpdate.Email = SelectedUser.Email;
                        userToUpdate.UpdatedAt = DateTime.Now;
                        context.SaveChanges();

                        MessageBox.Show("L'utilisateur a été mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Impossible de trouver l'utilisateur à mettre à jour.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la mise à jour de l'utilisateur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
