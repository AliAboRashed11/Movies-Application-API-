using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.DAL.Data.DbHelper;
using Movies.DAL.Repositories.UnitOfWork;
using MoviesApi.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        

        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedPosterSize = 1048576;
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMovies()
        {

            var movies = await _unitOfWork.Movie.GetAllAsync(Includeword: "Genre");
            var movieDtos = movies.Select(a => new MoviesDetailsDto
            {
                Id = a.Id,
                GenreId = a.GenreId,
                GenreName = a.Genre.Name,
                Title = a.Title,
                Poster = a.Poster,
                Year = a.Year,
                Rate = a.Rate,
            }).OrderByDescending(a => a.Rate).ToList();

            return Ok(movieDtos);

        }

        [HttpPost]
        public async Task<IActionResult> AddMoviesAsync([FromForm] MoviesDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required.");
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if (dto.Poster.Length > _MaxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = _unitOfWork.Genres.GetFirstorDefault( dto.GenreId);
            if (isValidGenre == null)
                return BadRequest("Invalid genre ID.");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                Storelien = dto.Storelien,
                Year = dto.Year,
            };

            await _unitOfWork.Movie.AddAsync(movie);
            await _unitOfWork.SaveChangesAsync();

            return Ok(movie);
        }

      

        [HttpGet("Movies/{genreId}")]
        public async Task<IActionResult> GetByGenreIdAsync(int genreId)
        {
            var movies = await _unitOfWork.Movie.GetAllAsync();
            var filteredMovies = movies.Where(a => a.GenreId == genreId)
                .OrderByDescending(a => a.Rate)
                .Select(a => new MoviesDetailsDto
                {
                    Id = a.Id,
                    GenreId = a.GenreId,
                    GenreName = a.Genre != null ? a.Genre.Name : "No Genre",
                    Title = a.Title,
                    Poster = a.Poster,
                    Year = a.Year,
                    Rate = a.Rate,
                }).ToList();

            return Ok(filteredMovies);
        }
        [HttpGet("{Id}")]
       
        public async Task<IActionResult> GetMovieById(int Id)
        {
            var movie =  _unitOfWork.Movie.GetFirstorDefault( Id,Includeword:"Genre");
            if (movie == null)
                return NotFound($"No Movie was found with ID {Id}");

            var movieDto = new MoviesDetailsDto
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre != null ? movie.Genre.Name : "No Genre",
                Title = movie.Title,
                Poster = movie.Poster,
                Year = movie.Year,
                Rate = movie.Rate
            };

            return Ok(movieDto);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateMovie(int Id, [FromForm] MoviesDto dto)
        {
            var movie =  _unitOfWork.Movie.GetFirstorDefault(Id);
            if (movie == null)
                return NotFound($"No movie was found with ID {Id}");

            var isValidGenre =  _unitOfWork.Genres.GetFirstorDefault(  dto.GenreId);
            if (isValidGenre == null)
                return BadRequest("Invalid genre ID");

            if (dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _MaxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;

            _unitOfWork.Movie.Update(movie);
            await _unitOfWork.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie =  _unitOfWork.Movie.GetFirstorDefault(  id);
            if (movie == null)
                return NotFound($"No movie was found with ID {id}");

            await _unitOfWork.Movie.Remove(movie);
            await _unitOfWork.SaveChangesAsync();

            return Ok(movie);
        }

    }
}
