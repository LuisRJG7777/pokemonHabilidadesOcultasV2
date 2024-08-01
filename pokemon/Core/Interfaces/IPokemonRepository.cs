using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPokemonRepository
    {
        Task<Pokemon> GetPokemonAsync(string pokemonName);
    }
}
