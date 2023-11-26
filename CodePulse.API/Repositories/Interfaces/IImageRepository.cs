using CodePulse.API.Models;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
