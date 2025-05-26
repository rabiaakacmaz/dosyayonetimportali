using DosyaYonetim.API.Data;
using DosyaYonetim.API.Entities;
using DosyaYonetim.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DosyaYonetim.API.Services.Implementations
{
    public class UserFileService : IUserFileService
    {
        private readonly AppDbContext _context;

        public UserFileService(AppDbContext context)
        {
            _context = context;
        }


        public async Task UploadFileAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Dosya geçersiz.");

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var filePath = Path.Combine(uploadFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var userFile = new UserFile
            {
                FileName = file.FileName,
                FilePath = "/uploads/" + file.FileName,
                UploadedAt = DateTime.UtcNow,
                AppUserId = userId
            };

            _context.UserFiles.Add(userFile);
            await _context.SaveChangesAsync();
        }


        public async Task<List<UserFile>> GetFilesByUserIdAsync(string userId)
        {
            return await _context.UserFiles
                .Where(f => f.AppUserId == userId)
                .ToListAsync();
        }


        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _context.UserFiles.FindAsync(fileId);
            if (file == null) return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _context.UserFiles.Remove(file);
            await _context.SaveChangesAsync();
        }
    }
}