using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;
using System.Threading.Tasks;
using System.Security.Claims;
using pokemon.Core.Entities;
using pokemon.Infrastructure.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/pokemon/habilidadesOcultas")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _pokemonService;

        public PokemonController(PokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetHiddenAbilities(string pokemonName)
        {
            // TOKEN 4
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = Jwt.validarToken(identity);

            if (!rToken.success)
            {
                return Unauthorized(new
                {
                    success = rToken.success,
                    message = rToken.message,
                    result = rToken.result
                });
            }

            EntrenadorPokemon usuario = rToken.result;

            if (usuario.rol == "administrador")
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "No tienes permisos",
                    result = ""
                });
            }

            var pokemonDto = await _pokemonService.GetHiddenAbilitiesAsync(pokemonName);
            return Ok(pokemonDto);
        }

    }

}

