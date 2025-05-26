using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using DosyaYonetimPorttali.MVC.Models;

public class DosyaController : Controller
{
    private readonly IHttpContextAccessor _accessor;
    private readonly HttpClient _httpClient;

    public DosyaController(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7151/api/");

        var token = _accessor.HttpContext.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("Dosyalar/KullaniciyaAit");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Hata = "Dosyalar alınamadı. Giriş yapmanız gerekebilir.";
            return View(new List<DosyaDto>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var dosyalar = JsonConvert.DeserializeObject<List<DosyaDto>>(json);
        return View(dosyalar);
    }
}