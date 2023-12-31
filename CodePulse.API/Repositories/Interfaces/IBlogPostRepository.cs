﻿using CodePulse.API.Models;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetById(Guid id);

        Task<BlogPost?> GetByUrlHandlerAsync(string urlHandler);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost> DeleteAsync(Guid id);
       
    }
}
