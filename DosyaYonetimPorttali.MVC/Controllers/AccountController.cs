using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly AuthService _authService = new();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid || model.Password != model.ConfirmPassword)
        {
            ViewBag.Error = "Şifreler uyuşmuyor veya alanlar eksik.";
            return View(model);
        }

        var result = await _authService.RegisterAsync(model);
        if (result)
            return RedirectToAction("Login");

        ViewBag.Error = "Kayıt başarısız.";
        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var token = await _authService.LoginAsync(model);
        if (!string.IsNullOrEmpty(token))
        {
            HttpContext.Session.SetString("token", token);
            return RedirectToAction("Index", "Admin"); 
        }

        ViewBag.Error = "Giriş başarısız.";
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}