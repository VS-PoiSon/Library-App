using System.Windows;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenresWindow : Window
    {
        private LibraryContext _context;

        public GenresWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }

        private void LoadData()
        {
            dgGenres.ItemsSource = _context.Genres.ToList();
        }

        private void btnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenreEditWindow();
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dgGenres.SelectedItem is Genre selected)
            {
                var window = new GenreEditWindow(selected.Id);
                if (window.ShowDialog() == true)
                    LoadData();
            }
            else
            {
                MessageBox.Show("Выберите жанр для редактирования.");
            }
        }

        private void btnDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dgGenres.SelectedItem is Genre selected)
            {
                var result = MessageBox.Show($"Удалить жанр '{selected.Name}'?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Genres.Remove(selected);
                    _context.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для удаления.");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
