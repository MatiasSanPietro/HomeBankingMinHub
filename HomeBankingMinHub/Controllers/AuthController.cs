using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Utils.Interfaces;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IHasher _hasher;
        public AuthController(IClientRepository clientRepository, IHasher hasher)
        {
            _clientRepository = clientRepository;
            _hasher = hasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientLoginDTO client)
        {
            try
            {
                // Validaciones para login
                if (string.IsNullOrEmpty(client.Email) || string.IsNullOrEmpty(client.Password))
                {
                    return StatusCode(400, "Todos los campos son obligatorios");
                }

                Client user = _clientRepository.FindByEmail(client.Email);

                if (user == null || !_hasher.VerifyPassword(client.Password, user.Password, user.Salt))
                {
                    return Unauthorized("Credenciales no validas"); // 401
                }

                //puedo agregar logica para checkear si el mail es admin, crear entidad usuario

                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok("Inicio de sesion exitoso");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok("Cierre de sesion exitoso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
