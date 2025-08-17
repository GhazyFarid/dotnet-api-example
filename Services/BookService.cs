using dotnet_api_example.Models;

namespace dotnet_api_example.Services
{
    public class BookService
    {
        private readonly List<Book> _books = new()
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Year = 2008 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Year = 1999 }
        };

        public List<Book> GetAll() => _books;

        public Book GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public Book AddBook(Book newBook)
        {
            newBook.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(newBook);
            return newBook;
        }

        public bool UpdateBook(int id, Book updateBook)
        {
            var book = GetById(id);
            if (book == null)
            {
                return false;
            }

            book.Title = updateBook.Title;
            book.Author = updateBook.Author;
            book.Year = updateBook.Year;
            return true;
        }

        public bool DeleteBook(int id)
        {
            var book = GetById(id);
            if (book == null)
            {
                return true;
            }

            _books.Remove(book);
            return true;
        }

    }
}
