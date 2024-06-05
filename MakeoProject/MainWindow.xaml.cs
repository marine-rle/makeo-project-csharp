using MakeoProject.Views;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MakeoProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Session.IsLoggedIn = false; // Initialiser l'état de connexion à false
            UpdateAuthButtons();
        }

        private void Home_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Home());
        }

        private void Freelances_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Freelances());
            }
            else
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette page.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Competences_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Competences());
            }
            else
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette page.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Statuts_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Statuts());
            }
            else
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette page.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Projects_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Projects());
            }
            else
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette page.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Users());
            }
            else
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette page.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
            if (Session.IsLoggedIn)
            {
                MessageBox.Show("Vous êtes connecté.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateAuthButtons();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Session.IsLoggedIn = false;
            MessageBox.Show("Vous êtes déconnecté.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateAuthButtons();
            ShowHomePage();
        }


        private void UpdateAuthButtons()
        {
            if (Session.IsLoggedIn)
            {
                LoginButton.Visibility = Visibility.Collapsed;
                RegisterButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
                RegisterButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowHomePage()
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Home());
        }
    }
}