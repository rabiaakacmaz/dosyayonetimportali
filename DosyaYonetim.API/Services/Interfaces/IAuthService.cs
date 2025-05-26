using DosyaYonetim.API.DTOs;
using System.Threading.Tasks;

namespace DosyaYonetim.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> RegisterAsync(RegisterDto dto);
        Task<object> LoginAsync(LoginDto dto);
    }
}