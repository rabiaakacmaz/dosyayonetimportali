using System.Security.Claims;
using System.Threading.Tasks;
using DosyaYonetim.API.Data; 
using DosyaYonetim.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DosyaYonetim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFileController : ControllerBase
    {
        private readonly IUserFileService _fileService;
        private readonly AppDbContext _context; 

        public UserFileController(IUserFileService fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bulunamadı.");

            if (file == null || file.Length == 0)
                return BadRequest("Geçersiz dosya.");

            var result = await _fileService.UploadFileAsync(file, userId);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yükleme başarısız.");

            return Ok("Dosya başarıyla yüklendi.");
        }

        [HttpGet("KullaniciyaAit")]
        public async Task<IActionResult> GetKullaniciDosyalari()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bulunamadı.");

            var dosyalar = await _context.Dosyalar
                                         .Where(d => d.KullaniciId == userId)
                                         .ToListAsync();

            return Ok(dosyalar);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _fileService.DeleteFileAsync(id);

            if (!result)
                return NotFound("Dosya bulunamadı veya silinemedi.");

            return Ok(" Dosya başarıyla silindi.");
        }
    }
}