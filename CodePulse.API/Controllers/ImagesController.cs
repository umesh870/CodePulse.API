using CodePulse.API.Models;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public IImageRepository ImageRepository { get; }
        public ImagesController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await ImageRepository.GetAll();
            var response=new List<BlogImageDto>();
            foreach (var item in images)
            {
                response.Add(new BlogImageDto() { 
                 Id=item.Id,
                  DateCreated=item.DateCreated,
                   FileExtension=item.FileExtension,
                    FileName=item.FileExtension,
                     Title=item.Title,
                      Url=item.Url
                });
            }
            return Ok(response); 
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string filename, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                // File Upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Title = title,
                    FileName = file.FileName,
                    DateCreated = DateTime.Now
                };
                blogImage= await ImageRepository.Upload(file, blogImage);
                // now we will convert this domain model t DTO
                var response = new BlogImageDto()
                {
                    Id = blogImage.Id,
                    DateCreated = blogImage.DateCreated,
                    Title = blogImage.Title,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size cannot be more than 10MB");
            }
        }

    }
}
