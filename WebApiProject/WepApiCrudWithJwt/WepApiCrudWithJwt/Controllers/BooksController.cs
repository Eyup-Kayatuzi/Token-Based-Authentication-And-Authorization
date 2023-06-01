using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepApiCrudWithJwt.Context;
using WepApiCrudWithJwt.DTOs;
using WepApiCrudWithJwt.Models;

namespace WepApiCrudWithJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ordinary user")]
    public class BooksController : ControllerBase
    {
        private readonly AuthorBookDbContext _context;

        public BooksController(AuthorBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(AddBookDto book)
        {

            if (ModelState.IsValid)
            {
                if (_context.Books == null)
                {
                    return Problem("Entity set 'AuthorBookDbContext.Books'  is null.");
                }
                var targetAuth = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorName == book.AuthorName && x.AuthorSurName == book.AuthorSurName);
                if (targetAuth != null)
                {
                    var newBook = new Book { AuthorId = targetAuth.Id, BookName = book.BookName, PageNumber = book.PageNumber };
                    await _context.Books.AddAsync(newBook);
                    await _context.SaveChangesAsync();

                    foreach (var item in book.BookSorts)
                    {
                        var targetSort = await _context.Sorts.FirstOrDefaultAsync(x => x.SortName == item);
                        if (targetSort == null)
                        {
                            var newSort = new Sort { SortName = item };
                            _context.Sorts.Add(newSort);
                            await _context.SaveChangesAsync();
                            _context.BookSorts.Add(new BookSort { BookId = newBook.Id, SortId = newSort.Id});
                        }
                        else
                        {
                            _context.BookSorts.Add(new BookSort { BookId = newBook.Id, SortId = targetSort.Id });
                        }
                        await _context.SaveChangesAsync();
                    }
                    //return Ok("Başarılı");
                    return CreatedAtAction("GetBook", new { id = newBook.Id }, newBook);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Error", Message="Bu yazar sistemde mevcut değildir"});
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { Status = "Error", Message = "Modeliniz uygun değildir." });
            }


        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
