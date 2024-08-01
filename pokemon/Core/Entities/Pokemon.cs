namespace Core.Entities
{
    public class Pokemon
    {
        public Ability[]? abilities { get; set; }
        public int status { get; set; } = 500;
    }

    public class Ability
    {
        internal bool isHidden;

        public Ability1? ability { get; set; }
        public bool? is_hidden { get; set; }
        public int? slot { get; set; }
    }

    public class Ability1
    {
        public string? name { get; set; }
        public string? url { get; set; }
    }
}
