using Movies.DAL.Data.DbHelper;
using Movies.DAL.Repositories.GenericRepository;
using Movies.DAL.Repositories.GenresRepositry;
using Movies.DAL.Repositories.MoviesRepostry;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = null;
        
        

        public GenresRepository Genres { get; private set; }

        public MoveRepository Movie { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Movie = new MoveRepository(_context);
            Genres = new GenresRepository(_context);


        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
