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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUserDataRepository _userDataRepository;

        public CountryController(ICountryRepository countryRepository, IUserDataRepository userDataRepository)
        {
            _countryRepository = countryRepository;
            _userDataRepository = userDataRepository;
        }

        // GET: api/country/countryId
        [HttpGet("{countryId}")]
        public IActionResult GetCountryById(int countryId)
        {
            return Ok(_countryRepository.GetCountryById(countryId));
        }

        // GET: api/country
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_countryRepository.GetAll());
        }


        [HttpPut]
        public IActionResult Put(Country country)
        {
            _countryRepository.UpdateCountry(country);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post(Country country)
        {
            _countryRepository.AddCountry(country);
            return CreatedAtAction("Get", new { id = country.Id, country });
        }

        [HttpDelete("{countryId}")]
        public IActionResult Delete(int countryId)
        {

            _countryRepository.DeleteCountry(countryId);
            return NoContent();

        }

    }
}
