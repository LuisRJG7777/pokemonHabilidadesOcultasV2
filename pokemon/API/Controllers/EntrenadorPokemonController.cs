using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using pokemon.Application.DTOs;
using pokemon.Core.Entities;
using pokemon.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pokemon.API.Controllers
{
    [ApiController]
    [Route("api/pokemon/token")]
    public class PokemonController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PokemonController));
        public IConfiguration _configuration;
        public PokemonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult IniciarSesion([FromBody] LoginRequest request)
        {
            string user = request.Usuario;
            string password = request.Password;

            // Lógica de servicios
            EntrenadorPokemon usuario = EntrenadorPokemon.DB().Where(x => x.usuario == user && x.password == password).FirstOrDefault();

            if (usuario == null)
            {
                log.Info($"/pokemon/token/login: Intento de inicio de sesión fallido para el usuario: {user}");
                return Unauthorized(new
                {
                    success = false,
                    error = "No has podido acceder a tu Pokedex"
                });
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("id", usuario.idUsuario),
        new Claim("usuario", usuario.usuario)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(4),
                signingCredentials: signIn
            );

            log.Info($"/pokemon/token/login: Inicio de sesión exitoso para el usuario: {user}");
            return Ok(new
            {
                success = true,
                message = "Has ingresado a tu pokedex",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

    }
}
