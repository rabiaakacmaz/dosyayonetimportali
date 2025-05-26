using DosyaYonetim.API.DTOs;
using DosyaYonetim.API.Entities;
using DosyaYonetim.API.JWT;
using DosyaYonetim.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DosyaYonetim.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<object> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new
                {
                    message = "Kayıt başarısız!",
                    errors = result.Errors
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(2)
            };
        }

        public async Task<object> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return "Kullanıcı bulunamadı" ;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
            {
                return "Giriş başarısız!" ;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(2)
            };
        }
    }
}