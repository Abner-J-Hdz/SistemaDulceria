using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost("/account/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (email == "admin@dulceria.com" && password == "admin123")
                {
                    //Añadimos los claims Usuario y Rol para tenerlos disponibles en la Cookie
                    //Podríamos obtenerlos de una base de datos.
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, "admin@dulceria.com"),
                        new Claim(ClaimTypes.Role, "vendedor")
                    };

                    //Creamos el principal
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    //Generamos la cookie. SignInAsync es un método de extensión del contexto.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    //Redirigimos a la Home
                    return LocalRedirect("/");

                }
                else
                    return LocalRedirect("/login/Invalid credentials");

            }
            catch (Exception ex)
            {
                return LocalRedirect("/login/Invalid credentials");
            }
        }

        [HttpGet("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

    }
}
