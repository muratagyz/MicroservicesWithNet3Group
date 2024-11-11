using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace keycloak.web.Controllers;

public class DataController(IDataProtectionProvider  dataProtectionProvider) : Controller
{
    public async Task<IActionResult> Index()
    {
        var dataProtection = dataProtectionProvider.CreateProtector("salting");
        
        var protectedData = dataProtection.Protect("Hello World");
        
        var unprotectedData = dataProtection.Unprotect(protectedData);

        var dataProtection2 = dataProtection.ToTimeLimitedDataProtector();
        
        dataProtection2.Protect("Hello World", TimeSpan.FromDays(1)); 
        
        return View();
    }
}