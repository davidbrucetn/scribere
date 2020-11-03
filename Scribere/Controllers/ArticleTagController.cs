using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribere.Models;
using Scribere.Repositories;

namespace Scribere.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticletagController : ControllerBase
    {

        private readonly IArticleTagRepository _articleTagRepository;
        private readonly IUserDataRepository _userDataRepository;

        public ArticletagController(IArticleTagRepository articleTagRepository, IUserDataRepository userDataRepository)
        {
            _articleTagRepository = articleTagRepository;
            _userDataRepository = userDataRepository;
        }
        // GET: api/articletag/article/articleId
        [HttpGet("article/{articleId}")]
        public IActionResult GetByArticleId(int articleId)
        {
            return Ok(_articleTagRepository.GetAll(articleId));
        }


        // GET api/articletag/articleTag.id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_articleTagRepository.GetArticleTag(id));
        }

        // POST: api/articletag
        [HttpPost]
        public IActionResult Post(ArticleTag articleTag)
        {
            _articleTagRepository.AddArticleTag(articleTag);
            return CreatedAtAction("Get", new { id = articleTag.Id }, articleTag);
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
