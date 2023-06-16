using JN.Ordersystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public interface IService<T>
    {
        /// <summary>
        /// Get all the entities
        /// </summary>
        /// <returns>A list with all the entities</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Get a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific entity</returns>
        Task<T?> GetById(int id);

        /// <summary>
        /// Create a new entity and add it to the list
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A newly created entity</returns>
        Task<T> Create(T entity);

        /// <summary>
        /// Update the entire entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>An updated entity</returns>
        Task<T?> Update(int id, T entity);

        /// <summary>
        /// Deletes a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        Task<bool> Delete(int id);
    }
}
