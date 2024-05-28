using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MakeoProject.Views
{
    public partial class AddCompetences : Window
    {
        public AddCompetences()
        {
            InitializeComponent();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            int length = Name.Text.Length;
            CharacterCount.Text = $"{length}/30 caractères";

            if (length > 30)
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;

            if (!string.IsNullOrWhiteSpace(name) && name.Length <= 30)
            {
                try
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        var newCompetence = new Competence
                        {
                            Name = name,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        context.Competences.Add(newCompetence);
                        context.SaveChanges();
                        MessageBox.Show("Compétence ajoutée avec succès.");
                        this.Close();
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Une erreur est survenue lors de l'ajout de la compétence : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Le nom de la compétence doit être rempli et contenir au maximum 30 caractères.");
            }
        }
    }
}
