using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribere.Repositories;
using Scribere.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Scribere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserDataRepository _userDataRepository;

        public CategoryController(ICategoryRepository categoryRepository, IUserDataRepository userDataRepository)
        {
            _categoryRepository = categoryRepository;
            _userDataRepository = userDataRepository;
        }

        // GET: api/category/categoryId
        [HttpGet("{categoryId}")]
        public IActionResult GetCategoryById(int categoryId)
        {
            
            return Ok(_categoryRepository.GetCategoryById(categoryId));
        }

        // GET: api/category
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(_categoryRepository.GetAll());
        }

        [HttpPut("{categoryId}")]
        public IActionResult Put(int categoryId, Category category)
        {
            _categoryRepository.UpdateCategory(category);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post(Category category)
        {
            _categoryRepository.AddCategory(category);
            return CreatedAtAction("Get", new { id = category.Id, category });
        }

        [HttpDelete("{categoryId}")]
        public IActionResult Delete(int categoryId)
        {

            _categoryRepository.DeleteCategory(categoryId);
            return NoContent();

        }
    }
}
