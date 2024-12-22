
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class MoviesDto
    {

      
        [MaxLength(length: 250)]
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }
        [MaxLength(length: 2500)]
        public string Storelien { get; set; }

        public IFormFile? Poster { get; set; }

        public int GenreId { get; set; }


    }
}
