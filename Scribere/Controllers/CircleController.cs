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
    public class CircleController : ControllerBase
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly ICircleRepository _circleRepository;

        public CircleController(ICircleRepository circleRepository, IUserDataRepository userDataRepository)
        {
            _circleRepository = circleRepository;
            _userDataRepository = userDataRepository;
        }

        [HttpGet("{firebaseUserId}")]
        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        // GET: api/circle
        [HttpGet]
        public IActionResult GetCircles()
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            return Ok(_circleRepository.GetAllCircles(userId));
        }

        // POST: api/circle
        [HttpPost]
        public IActionResult Post(Circle circle)
        {
            UserData currentUser = GetCurrentUserData();
            circle.UserId = currentUser.Id;
            _circleRepository.AddCircle(circle);
            return CreatedAtAction("Get", new { id = circle.Id, circle });
        }

        // DELETE: api/circle
        [HttpDelete("{friendId}")]
        public IActionResult DeleteFavAuthor(int friendId)
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            _circleRepository.DeleteCircle(userId, friendId);
            return NoContent();

        }



    }
}
