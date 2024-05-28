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
    public partial class AddStatuts : Window
    {
        public AddStatuts()
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
                        var newStatut = new Statut
                        {
                            Name = name,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        context.Statuts.Add(newStatut);
                        context.SaveChanges();
                        MessageBox.Show("Statut ajouté avec succès.");
                        this.Close();
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Une erreur est survenue lors de l'ajout du statut : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Le nom du statut doit être rempli et contenir au maximum 30 caractères.");
            }
        }
    }
}
