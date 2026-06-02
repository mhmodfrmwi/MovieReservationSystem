using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file has been uploaded");

            var wwwRootPath = webHostEnvironment.WebRootPath;

            if (string.IsNullOrEmpty(wwwRootPath))
            {
                throw new Exception("Web root path is not configured.");
            }

            var uploadsFolder = Path.Combine(wwwRootPath, "images", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{folderName}/{uniqueFileName}";
        }
    }
}
