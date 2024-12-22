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
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _unitOfWork.Genres.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenresDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.SaveChangesAsync();
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GenresDto genreDto)
        {
            var genre =  _unitOfWork.Genres.GetFirstorDefault( id);
            if (genre == null)
                return NotFound(new { Message = $"No genre found with ID {id}" });

            genre.Name = genreDto.Name;
            await _unitOfWork.SaveChangesAsync();
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre =  _unitOfWork.Genres.GetFirstorDefault(id);
            if (genre == null)
                return NotFound(new { Message = $"No genre found with ID {id}" });

            _unitOfWork.Genres.Remove(genre);
            await _unitOfWork.SaveChangesAsync();
            return Ok(genre);
        }
    }

}

