using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scribere.Repositories;
using Scribere.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scribere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserDataRepository _userDataRepository;

        public ArticleController(IArticleRepository articleRepository, IUserDataRepository userDataRepository)
        {
            _articleRepository = articleRepository;
            _userDataRepository = userDataRepository;
        }

        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        // GET: api/article
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_articleRepository.GetAllArticles());
        }

        // GET: api/articles/mywriting
        [HttpGet("mywriting")]
        public IActionResult GetMyArticles()
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            return Ok(_articleRepository.GetAllArticlesByUserId(userId));
        }

        // GET: api/article/favorite
        [HttpGet("favorite")]
        public IActionResult GetFavorite()
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            return Ok(_articleRepository.GetFavoriteArticles(userId));
        }

        // GET api/article/article.id
        [HttpGet("{articleId}")]
        public IActionResult Get(int articleId)
        {
            return Ok(_articleRepository.GetArticleByArticleId(articleId));
        }

        // GET api/article/author/article.UserId
        [HttpGet("author/{articleUserId}")]
        public IActionResult GetAllArticlesByUserId(int articleUserId)
        {
            return Ok(_articleRepository.GetAllArticlesByUserId(articleUserId));
        }


        // POST api/article
        [HttpPost]
        public IActionResult Post(Article article)
        {
            _articleRepository.Add(article);
            return CreatedAtAction("Get", new { id = article.Id }, article);
        }

        // PUT api/article/articleId
        [HttpPut("{articleId}")]
        public IActionResult Put(int articleId, Article article)
        {
            var currentUser = GetCurrentUserData();
            if (currentUser.Id != article.UserId)
            {
                return Unauthorized();
            }
            if (article.Id != articleId)
            {
                return BadRequest();
            }

            _articleRepository.UpdateArticle(article);
            return NoContent();
        }

        // DELETE api/article/5
        [HttpDelete("{articleId}")]
        public IActionResult Delete(int articleId)
        {
            _articleRepository.DeleteArticle(articleId);
            return NoContent();
        }
    }
}
