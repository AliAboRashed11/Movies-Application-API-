using Microsoft.EntityFrameworkCore;
using Movies.DAL.Data.DbHelper;
using Movies.DAL.Repositories.GenericRepository;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.MoviesRepostry
{
    public class MoveRepository : GenericRepository<Movie>, ImoveRepostry
    {
        private readonly ApplicationDbContext _context;

        public MoveRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Movie>> GetByGenreIdAsync(int genreId)
        {
            return await _context.Movies
          .Where(m => m.GenreId == genreId)
          .Include(m => m.Genre)
          .ToListAsync();
        }

       
    }
}
