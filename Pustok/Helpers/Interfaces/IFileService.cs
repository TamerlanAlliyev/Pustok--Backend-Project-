using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Pustok.Helpers.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveToAsync(IFormFile file, string folderPath);
        bool FileTypeAsync(IFormFile file, string fileType);
        bool FileSizeAsync(IFormFile file, int size);
    }
}
