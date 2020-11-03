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
    public class UserBlockController : ControllerBase
    {
        private readonly IUserBlockRepository _userBlockRepository;
        private readonly IUserDataRepository _userDataRepository;

        public UserBlockController(IUserBlockRepository userBlockRepository, IUserDataRepository userDataRepository)
        {
            _userBlockRepository = userBlockRepository;
            _userDataRepository = userDataRepository;
        }

        [HttpGet("{firebaseUserId}")]
        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        // GET: api/userblock
        [HttpGet]
        public IActionResult Get()
        {
            UserData currentUser = GetCurrentUserData();
            int userId = currentUser.Id;
            return Ok(_userBlockRepository.GetAll());
        }

        // POST: api/userblock
        [HttpPost]
        public IActionResult Post(UserBlock userblock)
        {
            _userBlockRepository.AddUserBlock(userblock);
            return CreatedAtAction("Get", new { id = userblock.Id, userblock });
        }

        // DELETE: api/userblock
        [HttpDelete("{userblockId}")]
        public IActionResult Delete(int userblockId)
        {
            UserData currentUser = GetCurrentUserData();
            int sourceUserId = currentUser.Id;
            _userBlockRepository.DeleteUserBlock(sourceUserId, userblockId);
            return NoContent();

        }


    }
}
