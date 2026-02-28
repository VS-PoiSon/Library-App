using System.Windows;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorsWindow : Window
    {
        private LibraryContext _context;

        public AuthorsWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }

        private void LoadData()
        {
            dgAuthors.ItemsSource = _context.Authors.ToList();
        }

        private void btnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthorEditWindow();
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (dgAuthors.SelectedItem is Author selected)
            {
                var window = new AuthorEditWindow(selected.Id);
                if (window.ShowDialog() == true)
                    LoadData();
            }
            else
            {
                MessageBox.Show("Выберите автора для редактирования.");
            }
        }

        private void btnDeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (dgAuthors.SelectedItem is Author selected)
            {
                var result = MessageBox.Show($"Удалить автора '{selected.FirstName} {selected.LastName}'?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Authors.Remove(selected);
                    _context.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для удаления.");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
