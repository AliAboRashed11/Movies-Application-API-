

namespace MoviesApi.Dtos
{
    public class MoviesDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }
      
        public string Storelien { get; set; }

        public byte[] Poster { get; set; }

        public int GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
