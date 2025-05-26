using System.Net.Http.Headers;
using Newtonsoft.Json;

public class DosyaService
{
    private readonly HttpClient _httpClient;

    public DosyaService(IHttpContextAccessor accessor)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7151/api/");
        var token = accessor.HttpContext.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<DosyaDto>> GetAllDosyalarAsync()
    {
        var response = await _httpClient.GetAsync("dosyalar");
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<DosyaDto>>(json);
    }
}