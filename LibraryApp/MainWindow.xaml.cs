using System.Windows;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }

        private void LoadData()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();

            dgBooks.ItemsSource = books;
            txtStatus.Text = $"Книг в библиотеке: {books.Count}";

            cmbAuthor.ItemsSource = _context.Authors.ToList();
            cmbAuthor.DisplayMemberPath = "LastName";

            cmbGenre.ItemsSource = _context.Genres.ToList();
            cmbGenre.DisplayMemberPath = "Name";
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            var window = new BookEditWindow();
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEditBook_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem is Book selectedBook)
            {
                var window = new BookEditWindow(selectedBook.Id);
                if (window.ShowDialog() == true)
                    LoadData();
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования.");
            }
        }

        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show($"Удалить книгу '{selectedBook.Title}'?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Books.Remove(selectedBook);
                    _context.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления.");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            if (!string.IsNullOrEmpty(txtSearch.Text))
                query = query.Where(b => b.Title.Contains(txtSearch.Text));

            if (cmbAuthor.SelectedItem is Author author)
                query = query.Where(b => b.AuthorId == author.Id);

            if (cmbGenre.SelectedItem is Genre genre)
                query = query.Where(b => b.GenreId == genre.Id);

            var books = query.ToList();
            dgBooks.ItemsSource = books;
            txtStatus.Text = $"Книг в библиотеке: {books.Count}";
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            cmbAuthor.SelectedItem = null;
            cmbGenre.SelectedItem = null;
            LoadData();
        }

        private void btnAuthors_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthorsWindow();
            window.ShowDialog();
            LoadData();
        }

        private void btnGenres_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenresWindow();
            window.ShowDialog();
            LoadData();
        }
    }
}
