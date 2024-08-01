using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Core.Entities;
using Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Services
{
    public class PokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        //LOG 1
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PokemonService(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        public async Task<PokemonDto> GetHiddenAbilitiesAsync(string pokemonName)
        {
            PokemonDto response = new PokemonDto();

            var _response = await _pokemonRepository.GetPokemonAsync(pokemonName);

            var abilities = Newtonsoft.Json.JsonConvert.SerializeObject(_response.abilities);

            if (_response.status == 200)
            {
                var jsonArray = JArray.Parse(abilities);

                // Filtrar habilidades donde "is_hidden" es verdadero
                var hiddenAbilities = new List<Oculta>();

                foreach (var ability in jsonArray.Children<JObject>())
                {
                    var isHidden = (bool)ability["is_hidden"];
                    if (isHidden)
                    {
                        var oculta = new Oculta
                        {
                            name = (string)ability["ability"]?["name"],
                            url = (string)ability["ability"]?["url"]
                        };
                        hiddenAbilities.Add(oculta);
                    }
                }

                var hiddenAbilitiesArray = hiddenAbilities.ToArray();


                // Asignar propiedades al objeto de respuesta
                response.Status = _response.status;
                response.habilidades = new Habilidades
                {
                    ocultas = hiddenAbilitiesArray
                };
            }
            else
            {
                response.Status = _response.status;
                response.error = "No importa qué pokémon seas te buscaré te encontraré y te atraparé";
            }
            //LOG 2
            
            log.Info($"log : /pokemon/habilidadesOcultas/{pokemonName}:{JsonConvert.SerializeObject(response)}");

            return response;
        }
    }
}

