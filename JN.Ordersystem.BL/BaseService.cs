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

        /// <summary>
        /// Get all the entities
        /// </summary>
        /// <returns>A list with all the entities</returns>
        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Get a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific entity</returns>
        public virtual async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Create a new entity and add it to the list
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A newly created entity</returns>
        public virtual async Task<T?> Create(T entity)
        {
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Updates the entire entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>An updated entity</returns>
        public virtual async Task<T?> Update(int productId, T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Deletes a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
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
