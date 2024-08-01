namespace pokemon.Infrastructure.Data
{
    public class EntrenadorPokemon
    {
        public string idUsuario { get; set; }
        public string usuario {  get; set; }
        public string password {  get; set; }
        public string rol {  get; set; }

        public static List<EntrenadorPokemon> DB() 
        {
            var list = new List<EntrenadorPokemon>()
            {
                new EntrenadorPokemon
                {
                idUsuario = "1",
                usuario = "AshKetchum",
                password = "101Pokemon",
                rol = "Administrador"
                },
                new EntrenadorPokemon
                {
                idUsuario = "2",
                usuario = "Misty",
                password = "TogepiForever",
                rol = "Colaborador"
                },
               new EntrenadorPokemon

                {
                idUsuario = "3",
                usuario = "Brock",
                password = "mesero2.0",
                rol = "Colaborador"
                }
            };
            return list;
        }

    }
}
