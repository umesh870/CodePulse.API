using CodePulse.API.Models;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            // incoming data from client to dto now we need to convert this dto's data into domain model class.
            var category = new Category()
            {
                Name = createCategoryRequestDto.Name,
                urlHandler = createCategoryRequestDto.urlHandler,

            };
            await categoryRepository.CreateAsync(category);

            // now we need to show the saved data to client so we need dto
            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                urlHandler = category.urlHandler
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categoryDomain= await categoryRepository.GetAllAsync();
            //Map domain model to dto
            var response = new List<CategoryDto>();
            foreach (var categoryObj in categoryDomain)
            {
                response.Add(new CategoryDto() { 
                 Id=categoryObj.Id,
                  Name=categoryObj.Name,
                   urlHandler=categoryObj.urlHandler
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid Id)
        {
            var categoryDomain = await categoryRepository.GetById(Id);
            if (categoryDomain == null)
            {
                return NotFound();
            }
            //Map domain model to dto
            var response = new CategoryDto()
            {
                Id = categoryDomain.Id,
                Name = categoryDomain.Name,
                urlHandler = categoryDomain.urlHandler
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
          
            var category = new Category()
            {
                Id=Id,
                Name = updateCategoryRequestDto.Name,
                urlHandler = updateCategoryRequestDto.urlHandler
            };
            category= await categoryRepository.UpdateAsync(category);

            if (category == null)
                return NotFound();

            // Convert from domain model to dto
            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                urlHandler = category.urlHandler
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var recordtobedeleted=await categoryRepository.DeleteAsync(Id);
            if (recordtobedeleted == null)
            { return NotFound(); }
            // convert domain to dto
            var response = new CategoryDto()
            {
                Id = recordtobedeleted.Id,
                Name = recordtobedeleted.Name,
                urlHandler = recordtobedeleted.urlHandler
            };

            return Ok(response);
        }


        }
}
