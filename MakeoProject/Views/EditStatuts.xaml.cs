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
    public partial class EditStatuts : Window
    {
        // Property to store the selected statut data
        public Statut SelectedStatut { get; set; }


        // Constructor accepting the selected statut data
        public EditStatuts(Statut selectedStatut)
        {
            InitializeComponent();
            SelectedStatut = selectedStatut;
            DataContext = this;
            FillFormWithData();
        }


        // Method to fill the form with data of the selected statut
        private void FillFormWithData()
        {
            Name.Text = SelectedStatut.Name;
            // Bind other controls as needed
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

        // Validation method for name
        private bool IsValidName(string name)
        {
            // Check if the name contains only letters (including accented letters) and spaces, and is 30 characters or less
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 30;
        }

        // Event handler for confirming changes
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;

            if (IsValidName(name))
            {
                // Update the statut data with the modified values
                SelectedStatut.Name = name;
                SelectedStatut.UpdatedAt = DateTime.Now;

                // Save the changes to the database
                try
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        context.Update(SelectedStatut);
                        context.SaveChanges();
                        MessageBox.Show("Modifications enregistrées avec succès.");
                        this.Close();
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Une erreur est survenue lors de la modification du statut : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nom valide ne dépassant pas 30 caractères.");
            }
        }
    }
}
