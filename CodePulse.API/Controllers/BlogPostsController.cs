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

        // GET: {apiBaseUrl}/api/blogposts/{id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            // Get the blogpost from repository
            var blogpost = await blogPostRepository.GetById(id);
            if (blogpost is null)
                return NotFound();

            // convert domain model to dto
            var response = new BlogPostDto()
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
        };
            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHandler}")]
        public async Task<IActionResult> GetBlogByUrlHandler([FromRoute] string urlHandler)
        {
            var blogpost = await blogPostRepository.GetByUrlHandlerAsync(urlHandler);
            if (blogpost == null)
                return NotFound();
            if (blogpost is null)
                return NotFound();

            // convert domain model to dto
            var response = new BlogPostDto()
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
            };
            return Ok(response);
        }

    

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            // convert from DTO to DOMAIN
            var blogPost = new BlogPost
            {
                Id=id,
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
            // Foreach loop for loop through all the categories
            foreach (var categoryid in request.Categories)
            {
                var existingcategories = await categoryRepository.GetById(categoryid);
                if (existingcategories is not null)
                {
                    blogPost.Categories.Add(existingcategories);
                }
            }
            // call Repository to update blogpost domain model
            var updatedBlogPost=await blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost == null)
                return NotFound();
            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                urlHandler = blogPost.urlHandler,
                Categories =blogPost.Categories.Select(x=>new CategoryDto { Id=x.Id,Name=x.Name, urlHandler=x.urlHandler}).ToList()
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost=await blogPostRepository.DeleteAsync(id);
            if(this.DeleteBlogPost==null)
            {
                return NotFound();
            }
            // Convert domain to dto
            var response = new BlogPostDto()
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                Content = deletedBlogPost.Content,
                Author = deletedBlogPost.Author,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                ShortDescription = deletedBlogPost.ShortDescription,
                IsVisible = deletedBlogPost.IsVisible,
                urlHandler = deletedBlogPost.urlHandler,
                PublishedDate = deletedBlogPost.PublishedDate,

            };
            return Ok(response);

        }
    }
}
