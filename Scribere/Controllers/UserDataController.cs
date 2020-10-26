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
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataRepository _userDataRepository;
        
        public UserDataController(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        [HttpGet("{firebaseUserId}")]
        private UserData GetCurrentUserData()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userDataRepository.GetByFirebaseUserId(firebaseUserId);
        }

        [HttpPost]
        public IActionResult Post(UserData userData)
        {
            userData.Created_at = DateTime.Now;
            _userDataRepository.Add(userData);
            return CreatedAtAction(
                nameof(GetCurrentUserData),
                new { firebaseUserId = userData.FirebaseUserId },
                userData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userDataRepository.GetAll());
        }


        [Authorize]
        [HttpGet("deactivated")]
        public IActionResult GetDeactivatedUsers()
        {
            return Ok(_userDataRepository.GetDeactivated());
        }



        [HttpGet("details/{id}")]
        public IActionResult Get(int id)
        {
            var user = _userDataRepository.GetById(id);
            if (user != null)
            {
                NotFound();
            }
            return Ok(user);
        }



        // POST Soft delete, moves User to a "Deactivated" group
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int Id)
        {
            try
            {
                _userDataRepository.DeleteUser(Id);
                return NoContent();
            }
            catch
            {
                UserData user = _userDataRepository.GetById(Id);
                user.Error = true;
                if (user != null)
                {
                    NotFound();
                }
                return Forbid();
            }
        }

        [HttpPut("reactivate/{id}")]
        public ActionResult Reactivate(int id)
        {
            try
            {
                _userDataRepository.ReactivateUser(id);
                return NoContent();
            }
            catch
            {
                return Forbid();
            }
        }

        [HttpPut("edittype")]
        public ActionResult Edit(UserData user)
        {
            try
            {
                _userDataRepository.UpdateUser(user);
                return NoContent();
            }
            catch
            {
                user = _userDataRepository.GetById(user.Id);
                user.Error = true;
                if (user != null)
                {
                    NotFound();
                }
                return Forbid();
            }
        }

    }
}
