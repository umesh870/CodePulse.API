using CodePulse.API.DATA;
using CodePulse.API.Models;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDBContext dbContext;

        public BlogPostRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.Blogposts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.Blogposts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                dbContext.Blogposts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

     

        public async  Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.Blogposts.Include(cat=>cat.Categories).ToListAsync();
            
        }

        public async Task<BlogPost> GetById(Guid id)
        {
             return await dbContext.Blogposts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public  Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            // return await dbContext.Blogposts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
            throw new NotImplementedException();
        }

        public  Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            //var existingBlogPost = await dbContext.Blogposts.Include(x => x.Categories)
            //    .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            //if (existingBlogPost == null)
            //{
            //    return null;
            //}

            //// Update BlogPost
            //dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //// Update Categories
            //existingBlogPost.Categories = blogPost.Categories;

            //await dbContext.SaveChangesAsync();

            //return blogPost;
            throw new NotImplementedException();
        }

      
    }
}