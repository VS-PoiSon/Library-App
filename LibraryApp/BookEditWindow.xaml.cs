using System.Windows;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class BookEditWindow : Window
    {
        private LibraryContext _context;
        private int? _bookId;

        public BookEditWindow(int? bookId = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
            _bookId = bookId;

            cmbAuthor.ItemsSource = _context.Authors.ToList();
            cmbAuthor.DisplayMemberPath = "LastName";

            cmbGenre.ItemsSource = _context.Genres.ToList();
            cmbGenre.DisplayMemberPath = "Name";

            if (bookId.HasValue)
            {
                Title = "Редактировать книгу";
                var book = _context.Books.Find(bookId.Value);
                if (book != null)
                {
                    txtTitle.Text = book.Title;
                    txtYear.Text = book.PublishYear.ToString();
                    txtISBN.Text = book.ISBN;
                    txtQuantity.Text = book.QuantityInStock.ToString();
                    cmbAuthor.SelectedValue = book.AuthorId;
                    cmbGenre.SelectedValue = book.GenreId;
                }
            }
            else
            {
                Title = "Добавить книгу";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название книги.");
                return;
            }
            if (cmbAuthor.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора.");
                return;
            }
            if (cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр.");
                return;
            }

            int year = 0;
            int.TryParse(txtYear.Text, out year);
            int quantity = 0;
            int.TryParse(txtQuantity.Text, out quantity);

            var selectedAuthor = (Author)cmbAuthor.SelectedItem;
            var selectedGenre = (Genre)cmbGenre.SelectedItem;

            if (_bookId.HasValue)
            {
                var book = _context.Books.Find(_bookId.Value);
                if (book != null)
                {
                    book.Title = txtTitle.Text;
                    book.PublishYear = year;
                    book.ISBN = txtISBN.Text;
                    book.QuantityInStock = quantity;
                    book.AuthorId = selectedAuthor.Id;
                    book.GenreId = selectedGenre.Id;
                }
            }
            else
            {
                _context.Books.Add(new Book
                {
                    Title = txtTitle.Text,
                    PublishYear = year,
                    ISBN = txtISBN.Text,
                    QuantityInStock = quantity,
                    AuthorId = selectedAuthor.Id,
                    GenreId = selectedGenre.Id
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
