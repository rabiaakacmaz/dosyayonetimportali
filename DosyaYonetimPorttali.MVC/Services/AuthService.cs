using System.Text;
using Newtonsoft.Json;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7151/api/");
    }

    public async Task<string?> LoginAsync(LoginModel model)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Auth/Login", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadAsStringAsync();
        dynamic json = JsonConvert.DeserializeObject(result);
        return json?.token;
    }

    public async Task<bool> RegisterAsync(RegisterModel model)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Auth/Register", content);
        return response.IsSuccessStatusCode;
    }
}