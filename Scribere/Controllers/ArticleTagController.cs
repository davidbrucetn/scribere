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
    public class ArticleTagController : ControllerBase
    {
        
        private readonly IArticleTagRepository _articleTagRepository;
        private readonly IUserDataRepository _userDataRepository;

        public ArticleTagController(IArticleTagRepository articleTagRepository, IUserDataRepository userDataRepository)
        {
            _articleTagRepository = articleTagRepository;
            _userDataRepository = userDataRepository;
        }

        // GET: api/articletag/article/articleId
        [HttpGet("article/{articleId}")]
        public IActionResult Get(int articleId)
        {
            return Ok(_articleTagRepository.GetAll(articleId));
        }

        // POST: api/articletag
        [HttpPost]
        public IActionResult Post(ArticleTag articleTag)
        {
            _articleTagRepository.AddArticleTag(articleTag);
            return CreatedAtAction("Get", new { id = articleTag.Id, articleTag });
        }

        // DELETE: api/articletag
        [HttpDelete("{articleTagId}")]
        public IActionResult Delete(int articleTagId)
        {
            _articleTagRepository.DeleteArticleTag(articleTagId);
            return NoContent();

        }
    }
}
