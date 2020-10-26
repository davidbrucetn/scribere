using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribere.Repositories;
using Scribere.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Scribere.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;

        }

        // GET: api/book/article/articleId
        [HttpGet("article/{articleId}")]
        public IActionResult GetBookByArticleId(int articleId)
        {
            return Ok(_bookRepository.GetBookByArticleId(articleId));
        }

        [HttpPut]
        public IActionResult Put(Book book)
        {
            _bookRepository.UpdateBook(book);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post(Book book)
        {
            _bookRepository.AddBook(book);
            return CreatedAtAction("Get", new { id = book.Id, book });
        }

        [HttpDelete("{bookId}")]
        public IActionResult Delete(int bookId)
        {

            _bookRepository.DeleteBook(bookId);
            return NoContent();
           
        }


    }
}
