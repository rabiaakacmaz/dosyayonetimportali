using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

public class AdminController : Controller
{
    private readonly IHttpContextAccessor _accessor;
    private readonly HttpClient _httpClient;

    public AdminController(IHttpContextAccessor accessor)
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

    public IActionResult Index()
    {
        return View();
    }
}