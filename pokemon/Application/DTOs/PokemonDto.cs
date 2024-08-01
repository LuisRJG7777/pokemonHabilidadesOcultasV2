namespace Application.DTOs
{
    public class PokemonDto
    {
        public Habilidades habilidades { get; set; }
        public int Status { get; set; }
        public string? error { get; set; }
    }

    public class Habilidades
    {
        public Oculta[] ocultas { get; set; }
    }

    public class Oculta
    {
        public string name { get; set; }
        public string url { get; set; }
    }
}
