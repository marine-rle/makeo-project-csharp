using MakeoProject.Views;
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
        }

        private void Home_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Home());
        }

        private void Freelances_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Freelances());
        }

        private void Competences_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Competences());
        }

        private void Statuts_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Statuts());
        }

        private void Projects_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Projects());
        }       
        
        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new Users());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}