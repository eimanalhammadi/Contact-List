using ContactsList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
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

namespace ContactsList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContactsDbContext _db;
        private ObservableCollection<Contact> _items;
      

        public MainWindow()
        {
            InitializeComponent();
            _db = new ContactsDbContext(); //user OnConfiguring connection
            this.Loaded += MainWindow_Loaded;

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAsync(); // fill the DataGrid with the contacts of the db
        }

        private async Task RefreshAsync(string nameFilter=null)
        {
            IQueryable<Contact> query = _db.Contacts;

            if(!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim();
                query = query.Where(c => c.FullName.Contains(term));
            }

            var list = await query
                .OrderBy(c => c.FullName)
                .ToListAsync();

            _items = new ObservableCollection<Contact>(list);
            ContactsGrid.ItemsSource = _items;
        }
        private async void SearchBtn_Click(object sender , RoutedEventArgs e)
        {
            await RefreshAsync(SearchBox.Text);
        }
        private async void SearchBox_KeyDown(object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
                await RefreshAsync(SearchBox.Text);
        }
        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddEditContactWindow();
            if (dlg.ShowDialog() == true)
            {
                //All fields are required; the dialog already validates empies
                _db.Contacts.Add(dlg.Model);
                await _db.SaveChangesAsync();
                await RefreshAsync(SearchBox.Text);
            }

        }
        private async void EditBtn_Click(Object sender, RoutedEventArgs e)
        {
            if (ContactsGrid.SelectedItem is not Contact selected)
            {
                MessageBox.Show("Select a contact to edit .", "Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //pass a copa into the dialog so cancel doesn't mutate the grid row
            var dlg = new AddEditContactWindow(selected);
            if (dlg.ShowDialog() == true)
            {
                //Write updated values back into the tracked entity
                _db.Entry(selected).CurrentValues.SetValues(dlg.Model);
                await _db.SaveChangesAsync();
                await RefreshAsync(SearchBox.Text);
            }
        }
        private async void DeleteBtn_Click(object sender , RoutedEventArgs e)
        {
            if (ContactsGrid.SelectedItem is not Contact selected)
            {
                MessageBox.Show("Select a contact to delete .", "Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete '{selected.FullName}'?",
                "Confirm delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                _db.Contacts .Remove(selected);
                await _db.SaveChangesAsync();
                await RefreshAsync(SearchBox.Text);
            }
        }
            
        



    }
}