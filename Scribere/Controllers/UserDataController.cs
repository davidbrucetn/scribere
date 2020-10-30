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
        public IActionResult GetUserData(string firebaseUserId)
        { 
            return Ok(_userDataRepository.GetByFirebaseUserId(firebaseUserId));
        }

        [HttpPost]
        public IActionResult Post(UserData userData)
        {
            userData.IsActive = 1;
            userData.AllowMessaging = 0;
            userData.UserLevelId = 2;
            userData.CountryId = 1;
            userData.CreateDate = DateTime.Now;
            _userDataRepository.Add(userData);
            return CreatedAtAction(
                nameof(GetUserData),
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



        // POST Soft delete
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

        // PUT api/userdata/userId
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, UserData userData)
        {
            _userDataRepository.UpdateUser(userData);
            return NoContent();
        }

    }
}
