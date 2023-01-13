using Datos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSistemaDulceria.Data.DulceriaService;
namespace WebSistemaDulceria.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbContextDulceria _context;

        public AccountController(DbContextDulceria context)
        {
            _context = context;
        }


        [HttpPost("/account/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == email);

                if (usuario == null)
                    return LocalRedirect("/login/usuario no encontrado");

                if (usuario.Password != password)
                    return LocalRedirect("/login/contraseña invalida");

                //Añadimos los claims Usuario y Rol para tenerlos disponibles en la Cookie
                //Podríamos obtenerlos de una base de datos.
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre + " " + usuario.Email),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

                //Creamos el principal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                //Generamos la cookie. SignInAsync es un método de extensión del contexto.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                //Redirigimos a la Home
                return LocalRedirect("/");

            }
            catch (Exception ex)
            {
                return LocalRedirect("/login/"+ex.Message);
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
