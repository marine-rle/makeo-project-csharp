using MakeoProject.DbLib.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MakeoProject.Views
{
    /// <summary>
    /// Logique d'interaction pour Users.xaml
    /// </summary>
    public partial class Users : UserControl
    {
        public ObservableCollection<User> alluser { get; set; }

        public User? SelectedUser { get; set; }

        public Users()
        {
            InitializeComponent();

            this.DataContext = this;

            using (MakeoProjectContext allusers = new())
            {
                alluser = new ObservableCollection<User>(allusers.Users.ToList());
            }

            var columnsToDisplay = new Dictionary<string, string>
            {
                { "Id", "ID" },
                { "Name", "Nom" },
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
                    default:
                        dataColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        break;
                }
                dgCustomer.Columns.Add(dataColumn);
            }

            dgCustomer.AutoGenerateColumns = false;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.Show();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.alluser != null && SelectedUser != null)
            {
                using (MakeoProjectContext context = new())
                {
                    context.Users.Remove(SelectedUser);
                    context.SaveChanges();
                    this.alluser.Remove(SelectedUser);
                }
            }
        }
    }
}
