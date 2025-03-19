namespace WebAPI_DOTNET8.DTOs
{
    public class BookUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? IdAuthor { get; set; }
    }
}
