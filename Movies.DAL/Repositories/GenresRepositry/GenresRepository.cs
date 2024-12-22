using Microsoft.EntityFrameworkCore;
using Movies.DAL.Data.DbHelper;
using Movies.DAL.Repositories.GenericRepository;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.GenresRepositry
{
    public class GenresRepository : GenericRepository<Genre>, IGenres
    {
        private readonly ApplicationDbContext _context;
        public GenresRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Genre> GetGenreByNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
        }
    }
}
