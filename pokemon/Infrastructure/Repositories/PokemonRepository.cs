using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly HttpClient _client;

        public PokemonRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<Pokemon> GetPokemonAsync(string pokemonName)
        {
            Pokemon resp = new Pokemon();
            //var response = await _client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonName}");
            //var json = JObject.Parse(response);
            var url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = await client.GetAsync(url);
                var input = response1.Content.ReadAsStringAsync().Result;

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    resp = Newtonsoft.Json.JsonConvert.DeserializeObject<Pokemon>(input);
                }
                resp.status = (int)response1.StatusCode;
            }
            return resp;

           // var abilities = json["abilities"].ToObject<Ability[]>();
            //return new Pokemon
            //{
            //    Abilities = abilities
            //};
        }
    }
}
