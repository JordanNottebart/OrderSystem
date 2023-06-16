using JN.Ordersystem.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public abstract class BaseService<T> where T : class
    {
        protected readonly DataContext _context;

        protected BaseService(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T?> Create(T entity)
        {
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> Update(int productId, T entity)
        {
            _context.Entry(entity).State= EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
