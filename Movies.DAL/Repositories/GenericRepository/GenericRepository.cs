using Microsoft.EntityFrameworkCore;
using Movies.DAL.Data.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet =_context.Set<T>();  
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

      

      

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (Includeword != null)
            {
                //_context.Products.Include("Category,Logos,Users)
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstorDefault( int id , Expression<Func<T, bool>>? perdicate = null, string? Includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (Includeword != null)
            {
                //_context.Products.Include("Category,Logos,Users)
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
          return  query.FirstOrDefault();
        }


        public async Task  Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

       
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        

     
    }
}
