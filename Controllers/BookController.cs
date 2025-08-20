using dotnet_api_example.Models;
using dotnet_api_example.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_api_example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var books = _bookService.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create(Book newBook)
        {
            var createdBook = _bookService.AddBook(newBook);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut]
        public IActionResult Update(int id, Book updateBook)
        {
            var result = _bookService.UpdateBook(id, updateBook);
            if (!result) { return NotFound(); }
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = (_bookService.DeleteBook(id));
            if (!result) { return NotFound(); }
            return Ok(result);
        }

    }
}
