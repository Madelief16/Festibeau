namespace Festibeau.database
{
    public class Festival
    {
        public int Id { get; internal set; }
        public string naam { get; internal set; }
        public int data { get; internal set; }
        public string beschrijving { get; internal set; }
        public string locatie { get; internal set; }

    }
}