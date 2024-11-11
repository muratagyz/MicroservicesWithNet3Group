using keycloak.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace keycloak.web.Controllers;

public class AuthController(AuthService authService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var username = "ahmet16";
        var password = "Password12*";
        
        var response = await authService.SignIn(username, password);
        
        return View();
    }
}