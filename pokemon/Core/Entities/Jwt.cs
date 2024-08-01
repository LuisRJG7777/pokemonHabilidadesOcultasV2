using pokemon.Infrastructure.Data;
using System.Security.Claims;

namespace pokemon.Core.Entities
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static dynamic validarToken(ClaimsIdentity identity) 
        {
            try 
            {
                if(identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar Token",
                        result = ""
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;

                EntrenadorPokemon usuario = EntrenadorPokemon.DB().FirstOrDefault(x => x.idUsuario == id);

                return new
                {
                    success = true,
                    message = "Registro encontrado",
                    result = usuario
                };
                
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }

    }
}
