using DosyaYonetim.API.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DosyaYonetim.API.Services.Interfaces
{
    public interface IUserFileService
    {
        Task UploadFileAsync(IFormFile file, string userId);
        Task<List<UserFile>> GetFilesByUserIdAsync(string userId);
        Task DeleteFileAsync(int fileId);
    }
}