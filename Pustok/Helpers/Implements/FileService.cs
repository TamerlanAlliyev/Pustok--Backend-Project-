using Pustok.Helpers.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pustok.Helpers.Implements
{
    public class FileService : IFileService
    {
        public async Task<string> SaveToAsync(IFormFile file, string folderPath)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException(nameof(folderPath));

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public bool FileTypeAsync(IFormFile file, string fileType)
        {
            if (file.ContentType.StartsWith(fileType))
            {
                return true;
            }
            return false;
        }

        public bool FileSizeAsync(IFormFile file, int size)
        {
            if (file.Length < size)
            {
                return true;
            }
            return false;
        }
    }
}
