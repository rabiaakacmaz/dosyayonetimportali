using DosyaYonetim.API.DTOs;
using DosyaYonetim.API.Entities;
using DosyaYonetim.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DosyaYonetim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result is string strResult && strResult == "Kayıt başarısız!")
                return BadRequest(new { message = strResult });

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result is string strResult && (strResult == "Kullanıcı bulunamadı" || strResult == "Giriş başarısız!"))
                return BadRequest(new { message = strResult });

            return Ok(result);
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UserInfoDto
            {
                Email = user.Email,
                Roles = roles.ToList()
            };

            return Ok(dto);
        }
    }
}