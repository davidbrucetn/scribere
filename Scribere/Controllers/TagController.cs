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
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserDataRepository _userDataRepository;

        public TagController(ITagRepository tagRepository, IUserDataRepository userDataRepository)
        {
            _tagRepository = tagRepository;
            _userDataRepository = userDataRepository;
        }

        // GET: api/tag/tagId
        [HttpGet("{articleId}")]
        public IActionResult GetTagByTagId(int tagId)
        {
            return Ok(_tagRepository.GetTagById(tagId));
        }

        // GET: api/tag
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_tagRepository.GetAll());
        }


        [HttpPut]
        public IActionResult Put(Tag tag)
        {
            _tagRepository.UpdateTag(tag);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post(Tag tag)
        {
            return CreatedAtAction("Get", new { id = tag.Id, tag });
        }

        [HttpDelete("{tagId}")]
        public IActionResult Delete(int tagId)
        {

            _tagRepository.DeleteTag(tagId);
            return NoContent();

        }
    }
}
