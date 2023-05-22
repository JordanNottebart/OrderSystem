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
        List<T> GetAll();

        /// <summary>
        /// Get a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific entity</returns>
        T? GetById(int id);

        /// <summary>
        /// Create a new entity and add it to the list
        /// </summary>
        /// <param name="c"></param>
        /// <returns>A newly created entity</returns>
        T Create(T entity);

        /// <summary>
        /// Update the entire entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>An updated entity</returns>
        T Update(int id, T entity);

        /// <summary>
        /// Deletes a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        bool Delete(int id);
    }
}
