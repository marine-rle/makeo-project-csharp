using MakeoProject.DbLib.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MakeoProject.Views
{
    public partial class Users : UserControl
    {
        public ObservableCollection<User> alluser { get; set; }
        public User? SelectedUser { get; set; }

        public Users()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadUsers();
            InitializeDataGridColumns();

            // Vérifier si l'utilisateur actuel est un administrateur
            bool isAdmin = Session.CurrentUser != null && Session.CurrentUser.Admin;

            // Afficher ou masquer les boutons en fonction du statut de l'administrateur
            EditButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            DeleteButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadUsers()
        {
            using (MakeoProjectContext context = new MakeoProjectContext())
            {
                var users = context.Users.ToList();
                alluser = new ObservableCollection<User>(users);
            }
        }

        private void RefreshDataGrid()
        {
            LoadUsers();
            dgCustomer.ItemsSource = alluser;
        }

        private void InitializeDataGridColumns()
        {
            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "Prénom" },
                { "Surname", "Nom" },
                { "Email", "Email" }
            };

            foreach (var column in columnsToDisplay)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = column.Value;
                dataColumn.Binding = new Binding(column.Key);
                switch (column.Key)
                {
                    case "Id":
                        dataColumn.Width = new DataGridLength(30);
                        break;
                    case "Name":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    case "Surname":
                        dataColumn.Width = new DataGridLength(150);
                        break;
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        break;
                }
                dgCustomer.Columns.Add(dataColumn);
            }

            dgCustomer.AutoGenerateColumns = false;
        }

        // Gestionnaire d'événement pour le clic sur le bouton Information
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomer.SelectedItem is User selectedUser)
            {
                var infoWindow = new InfoUser(selectedUser);
                infoWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur pour voir les détails.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Gestionnaire d'événement pour le clic sur le bouton Modifier
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                EditUser editUser = new EditUser(SelectedUser);
                editUser.Closed += (s, args) => RefreshDataGrid(); // Actualiser le DataGrid après la fermeture de la fenêtre d'édition
                editUser.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à modifier.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Gestionnaire d'événement pour le clic sur le bouton Supprimer
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet utilisateur et tous ses projets ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (MakeoProjectContext context = new MakeoProjectContext())
                    {
                        var userToDelete = context.Users
                                                  .Include(u => u.Projects)
                                                  .ThenInclude(p => p.ProjetCompetences)
                                                  .Include(u => u.Projects)
                                                  .ThenInclude(p => p.FreelanceProjects)
                                                  .FirstOrDefault(u => u.Id == SelectedUser.Id);

                        if (userToDelete != null)
                        {
                            foreach (var project in userToDelete.Projects)
                            {
                                context.ProjetCompetences.RemoveRange(project.ProjetCompetences);
                                context.FreelanceProjects.RemoveRange(project.FreelanceProjects);
                            }

                            context.Projects.RemoveRange(userToDelete.Projects);
                            context.Users.Remove(userToDelete);
                            context.SaveChanges();
                        }
                    }

                    alluser.Remove(SelectedUser);
                    SelectedUser = null;
                    RefreshDataGrid(); // Actualiser le DataGrid après suppression
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
