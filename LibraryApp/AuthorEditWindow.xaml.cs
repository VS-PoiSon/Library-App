using System.Windows;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorEditWindow : Window
    {
        private LibraryContext _context;
        private int? _authorId;

        public AuthorEditWindow(int? authorId = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
            _authorId = authorId;

            if (authorId.HasValue)
            {
                Title = "Редактировать автора";
                var author = _context.Authors.Find(authorId.Value);
                if (author != null)
                {
                    txtFirstName.Text = author.FirstName;
                    txtLastName.Text = author.LastName;
                    dpBirthDate.SelectedDate = author.BirthDate;
                    txtCountry.Text = author.Country;
                }
            }
            else
            {
                Title = "Добавить автора";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите имя и фамилию автора.");
                return;
            }

            if (_authorId.HasValue)
            {
                var author = _context.Authors.Find(_authorId.Value);
                if (author != null)
                {
                    author.FirstName = txtFirstName.Text;
                    author.LastName = txtLastName.Text;
                    author.BirthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue;
                    author.Country = txtCountry.Text;
                }
            }
            else
            {
                _context.Authors.Add(new Author
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    BirthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue,
                    Country = txtCountry.Text
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
