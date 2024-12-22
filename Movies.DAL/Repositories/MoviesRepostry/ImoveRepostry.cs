using Movies.DAL.Repositories.GenericRepository;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.MoviesRepostry
{
  public interface ImoveRepostry : IGenericRepository<Movie>
    {
        
        Task<IEnumerable<Movie>> GetByGenreIdAsync(int genreId);
       
    }
}
