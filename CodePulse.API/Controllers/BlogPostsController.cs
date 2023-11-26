using CodePulse.API.Models.DTOs;
using CodePulse.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Repositories.Interfaces;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,
                                   ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            // client passes new data in dto i.o. request and here we get data from request to domain for save purpose 
            // Convert DTO to DOmain
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                urlHandler = request.urlHandler,
                Categories = new List<Category>()
            };
            foreach (var categoryid in request.Categories)
            {
                var existingcategories = await categoryRepository.GetById(categoryid);
                if (existingcategories is not null)
                {
                    blogPost.Categories.Add(existingcategories);
                }
            }

            // now we need to call service for save
            blogPost = await blogPostRepository.CreateAsync(blogPost);
            // to show to outside world we need dto
            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                urlHandler = blogPost.urlHandler,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id=x.Id,Name=x.Name,urlHandler=x.urlHandler }).ToList()
            };
            return Ok(response);


        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPost= await blogPostRepository.GetAllAsync();
            // convert domain model to dto
            var response = new List<BlogPostDto>();
            foreach (var blogpost in blogPost)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogpost.Id,
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    PublishedDate = blogpost.PublishedDate,
                    ShortDescription = blogpost.ShortDescription,
                    Title = blogpost.Title,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    IsVisible = blogpost.IsVisible,
                    urlHandler = blogpost.urlHandler,
                    Categories = blogpost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, urlHandler = x.urlHandler }).ToList()
                });
            }
            return Ok(response);


        }
    }
}
