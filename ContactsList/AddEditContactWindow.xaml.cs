using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ContactsList.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ContactsList
{
    /// <summary>
    /// Interaction logic for AddEditContWindow.xaml
    /// </summary>
    /// 

    public partial class AddEditContactWindow : Window
    {
        public Contact Model { get; private set; }

        public AddEditContactWindow(Contact existing = null)
        {
            InitializeComponent();

            //Work with a copy so Cancel is safe
            Model = existing == null
                ? new Contact()
                : new Contact
                {
                    Id = existing.Id,
                    FullName = existing.FullName,
                    Email = existing.Email,
                    Phone = existing.Phone,
                    City = existing.City,
                };
            DataContext = Model;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //Enforce "all required" at UI level too
            if(string.IsNullOrWhiteSpace(Model.FullName)||
                string.IsNullOrWhiteSpace(Model.Email)||
                string.IsNullOrWhiteSpace(Model.Phone)||
                 string.IsNullOrWhiteSpace(Model.City))
            {
                MessageBox.Show("All fields are required." , "Validation" , MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;

        }
    }
    
    
        
    
}
