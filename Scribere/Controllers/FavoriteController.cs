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
    public class FavoriteController : ControllerBase
    {

        private readonly IUserDataRepository _userDataRepository;
        private readonly IFavoriteArticleRepository _favoriteArticleRepository;
        private readonly IFavoriteAuthorRepository _favoriteAuthorRepository;

        public FavoriteController(IFavoriteAuthorRepository favoriteAuthorRepository, IFavoriteArticleRepository favoriteArticleRepository, IUserDataRepository userDataRepository)
        {
            _favoriteAuthorRepository = favoriteAuthorRepository;
            _favoriteArticleRepository = favoriteArticleRepository;
            _userDataRepository = userDataRepository;
        }

        [HttpGet("{firebaseUserId}")]
        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        // GET: api/favorite/favauthor
        [HttpGet("favauthor")]
        public IActionResult GetFaveAuthors()
        {
            UserData currentUser = GetCurrentUserData();
            int sourceUserId = currentUser.Id;
            return Ok(_favoriteAuthorRepository.GetAll(sourceUserId));
        }

        // POST: api/favorite/favauthor
        [HttpPost("favauthor")]
        public IActionResult Post(FavoriteAuthor favoriteauthor)
        {
            UserData currentUser = GetCurrentUserData();
            favoriteauthor.SourceUserId = currentUser.Id;
            _favoriteAuthorRepository.AddFavoriteAuthor(favoriteauthor);
            return CreatedAtAction("Get", new { id = favoriteauthor.Id, favoriteauthor });
        }

        // DELETE: api/favorite/favauthor
        [HttpDelete("favauthor/{favoriteUserId}")]
        public IActionResult DeleteFavAuthor(int favoriteUserId)
        {
            UserData currentUser = GetCurrentUserData();
            int sourceUserId = currentUser.Id;
            _favoriteAuthorRepository.DeleteFavoriteAuthor(sourceUserId, favoriteUserId);
            return NoContent();

        }

        // GET: api/favorite/favarticle
        [HttpGet("favarticle")]
        public IActionResult GetFavArticles()
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            return Ok(_favoriteArticleRepository.GetAll(userId));
        }

        // POST: api/favorite/favarticle
        [HttpPost("favarticle")]
        public IActionResult Post(FavoriteArticle favoritearticle)
        {
            UserData currentUser = GetCurrentUserData();
            favoritearticle.UserId = currentUser.Id;
            _favoriteArticleRepository.AddFavoriteArticle(favoritearticle);
            return CreatedAtAction("Get", new { id = favoritearticle.Id, favoritearticle });
        }

        // DELETE: api/favorite/favarticle
        [HttpDelete("favarticle/{favoriteArticleId}")]
        public IActionResult DeleteFavArticle(int favoriteArticleId)
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            _favoriteArticleRepository.DeleteFavoriteArticle(userId, favoriteArticleId);
            return NoContent();

        }
    }
}
