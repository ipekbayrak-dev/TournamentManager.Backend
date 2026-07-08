namespace TournamentManager.Application.Common
{
    public class Paginate <T> where T : class
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public ICollection<T> Items { get; set; } = new List<T>();
        public bool HasPrevious => Index > 0;
        public bool HasNext => Index + 1 < Pages;
    }
}