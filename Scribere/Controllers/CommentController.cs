using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribere.Models;
using Scribere.Repositories;

namespace Scribere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserDataRepository _userDataRepository;

        public CommentController(ICommentRepository commentRepository, IUserDataRepository userDataRepository)
        {
            _commentRepository = commentRepository;
            _userDataRepository = userDataRepository;
        }

        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        // GET api/comment/comment.id
        [HttpGet("{commentId}")]
        public IActionResult Get(int commentId)
        {
            return Ok(_commentRepository.GetCommentByCommentId(commentId));
        }

        // GET api/comment/article/articleId
        [HttpGet("article/{articleId}")]
        public IActionResult GetAllCommentsByArticle(int articleId)
        {
            return Ok(_commentRepository.GetAllCommentsByArticleId(articleId));
        }

        // POST api/comment
        [HttpPost]
        public IActionResult Post(Comment comment)
        {
            _commentRepository.AddComment(comment);
            return CreatedAtAction("Get", new { id = comment.Id }, comment);
        }

        // PUT api/comment/commentId
        [HttpPut("{commentId}")]
        public IActionResult Put(int commentId, Comment comment)
        {
            _commentRepository.UpdateComment(comment);
            return NoContent();
        }

        // DELETE api/comment/5
        [HttpDelete("{commentId}")]
        public IActionResult Delete(int commentId)
        {
            _commentRepository.DeleteComment(commentId);
            return NoContent();
        }

    }
}
