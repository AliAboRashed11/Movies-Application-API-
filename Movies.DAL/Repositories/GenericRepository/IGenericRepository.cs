using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.GenericRepository
{
public interface IGenericRepository<T> where T : class
    {

       T GetFirstorDefault (int id,Expression<Func<T, bool>>? perdicate = null, string? Includeword = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null);
      
        Task AddAsync(T entity);
      
        void Update(T entity);
       Task Remove(T entity);
        
    }
}
