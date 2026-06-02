using Microsoft.AspNetCore.Http;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
    }
}
