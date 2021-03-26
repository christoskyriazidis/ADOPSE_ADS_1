using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface IService<T> {

        /// <summary>
        /// Returns the entity of type T that has the given id.
        /// </summary>
        /// <param name="id"> The id of the entity to read.</param>
        /// <returns>Entity that has the given id or exception(if no entity found)</returns>
        T ReadById(long id);

        /// <summary>
        /// Updates the given entity.
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);

        /// <summary>
        /// Deletes the given entity
        /// </summary>
        /// <param name="t"></param>
        void Delete(T t);
        
        /// <summary>
        /// Creates new entity.
        /// </summary>
        /// <param name="t"></param>
        void Create(T t);

        /// <summary>
        /// Returns a scroller over the entities that are stored.
        /// </summary>
        /// <returns></returns>
        IScroller<T> Scroller();

    }
}
