using Microsoft.EntityFrameworkCore;
using Movies.DAL.Repositories.GenericRepository;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.GenresRepositry
{
    public interface IGenres : IGenericRepository<Genre>
    {
       
            Task<Genre> GetGenreByNameAsync(string name);
        
    }
}
