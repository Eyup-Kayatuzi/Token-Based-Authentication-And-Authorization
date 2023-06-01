namespace WepApiCrudWithJwt.Models
{
    public class BookSort
    {
        public int Id { get; set; }
        public int BookId { get; set; } 
        public virtual Book Book { get; set; }

        public int SortId { get; set; } 
        public virtual Sort Sort { get; set; }

    }
}
