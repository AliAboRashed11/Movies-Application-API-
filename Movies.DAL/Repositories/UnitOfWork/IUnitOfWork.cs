
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
    public interface IUnitOfWork : IDisposable
    {
      GenresRepository Genres { get; }
      MoveRepository Movie { get; }


        Task<int> SaveChangesAsync();
    }
}
