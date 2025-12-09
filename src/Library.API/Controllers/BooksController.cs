using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Exceptions;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { message = $"Libro con ID {id} no encontrado." });
            
            return Ok(book);
        }

        // POST: api/books (Requisito 4.1.2)
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            try
            {
                var book = await _bookService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
        }

        // PUT: api/books/{id} (Requisito 4.1.3)
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto dto)
        {
            try
            {
                var book = await _bookService.UpdateAsync(id, dto);
                return Ok(book);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
        }

        // DELETE: api/books/{id} (Requisito 4.1.4)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
            catch (BusinessRuleException ex)
            {

                return BadRequest(new { message = ex.Message }); 
            }
        }
    }
}