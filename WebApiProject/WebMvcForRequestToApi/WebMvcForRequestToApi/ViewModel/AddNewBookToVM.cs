namespace WebMvcForRequestToApi.ViewModel
{
    public class AddNewBookToVM
    {
        public string BookName { get; set; }
        public int PageNumber { get; set; }
        public string AuthorName { get; set; } = null!;
        public string AuthorSurName { get; set; } = null!;
    }
}
