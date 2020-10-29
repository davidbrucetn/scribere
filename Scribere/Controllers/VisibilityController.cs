using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribere.Models;
using Scribere.Repositories;

namespace Scribere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisibilityController : ControllerBase
    {
        private readonly IVisibilityRepository _visibilityRepository;
        private readonly IUserDataRepository _userDataRepository;

        public VisibilityController(IVisibilityRepository visibilityRepository, IUserDataRepository userDataRepository)
        {
            _visibilityRepository = visibilityRepository;
            _userDataRepository = userDataRepository;
        }

        // GET: api/visibility/visibilityId
        [HttpGet("{articleId}")]
        public IActionResult GetVisibilityByVisibilityId(int visibilityId)
        {
            return Ok(_visibilityRepository.GetVisibilityById(visibilityId));
        }

        // GET: api/visibility
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_visibilityRepository.GetAllVisibilities());
        }


        [HttpPut]
        public IActionResult Put(Visibility visibility)
        {
            _visibilityRepository.UpdateVisibility(visibility);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post(Visibility visibility)
        {
            return CreatedAtAction("Get", new { id = visibility.Id, visibility });
        }

        [HttpDelete("{visibilityId}")]
        public IActionResult Delete(int visibilityId)
        {

            _visibilityRepository.DeleteVisibility(visibilityId);
            return NoContent();

        }
    }
}
