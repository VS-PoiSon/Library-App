using System.Windows;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenreEditWindow : Window
    {
        private LibraryContext _context;
        private int? _genreId;

        public GenreEditWindow(int? genreId = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
            _genreId = genreId;

            if (genreId.HasValue)
            {
                Title = "Редактировать жанр";
                var genre = _context.Genres.Find(genreId.Value);
                if (genre != null)
                {
                    txtName.Text = genre.Name;
                    txtDescription.Text = genre.Description;
                }
            }
            else
            {
                Title = "Добавить жанр";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название жанра.");
                return;
            }

            if (_genreId.HasValue)
            {
                var genre = _context.Genres.Find(_genreId.Value);
                if (genre != null)
                {
                    genre.Name = txtName.Text;
                    genre.Description = txtDescription.Text;
                }
            }
            else
            {
                _context.Genres.Add(new Genre
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text
                });
            }

            _context.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
