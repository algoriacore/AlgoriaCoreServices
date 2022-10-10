
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IRepository<TEntity, TType> : IRepository
        where TEntity : class, IEntity<TType>
    {
        #region Select/Get/Query

        /// <summary>
        /// Usada para obtener un IQueryable que es usado para recuperar todas las entidades de una tabla
        /// </summary>
        /// <returns>IQueryable para ser usado para seleccionar entidades de una base de datos</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Usada para obtener un IQueryable que es usado para recuperar todas las entidades de una tabla
        /// Uno o más
        /// </summary>
        /// <param name="propertySelectors">Una lista de expresiones de inclusión.</param>
        /// <returns>IQueryable para ser usado para seleccionar entidades de una base de datos</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Usado para obtener todas las entidades.
        /// </summary>
        /// <returns>Lista de todas las entidades</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// Usado para obtener todas las entidades basadas en <paramref name="predicate"/> dado.
        /// </summary>
        /// <param name="predicate">Una condición para filtrar entidades</param>
        /// <returns>Lista de todas las entidades</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Usado para obtener todas las entidades.
        /// </summary>
        /// <returns>Lista de todas las entidades</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Usado para obtener todas las entidades basado en un <paramref name="predicate"/> dado.
        /// </summary>
        /// <param name="predicate">Una condición para filtrar entidades</param>
        /// <returns>Lista de todas las entidades</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Usado para ejecutar una consulta sobre todas las entidades.
        /// Atributo <see cref="UnitOfWorkAttribute"/> no siempre es necesario (opuesto a <see cref="GetAll"/>)
        /// si <paramref name="queryMethod"/> IQueryable termina con ToList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="T">Tipo del valor de retirno de este método</typeparam>
        /// <param name="queryMethod">Este método es usado para realizar consulta sobre las entidades</param>
        /// <returns>Resultado de la consulta</returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// Obtiene una entidad dada una clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad dada</param>
        /// <returns>Entidad</returns>
        TEntity Get(TType id);

        /// <summary>
        /// Obtiene una entidad dada una clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad dada</param>
        /// <returns>Entidad</returns>
        Task<TEntity> GetAsync(TType id);

        /// <summary>
        /// Obtiene exactamente una entidad con el predicado dado.
        /// Lanza excepción si no hay entidad o hay más de una.
        /// </summary>
        /// <param name="predicate">Entidad</param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene exactamente una entidad con el predicado dado.
        /// Lanza excepción si no hay entidad o hay más de una.
        /// </summary>
        /// <param name="predicate">Entidad</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene una entidad con la clave primaria dada o nulo si no la encuentra.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad a obtener</param>
        /// <returns>Entidad o nulo</returns>
        TEntity FirstOrDefault(TType id);

        /// <summary>
        /// Obtiene una entidad con el predicado dado o nulo si no la encuentra.
        /// </summary>
        /// <param name="predicate">Predicado para filtrar las entidades</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene una entidad con la clave primaria dada o nulo si no la encuentra.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad a obtener</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(TType id);

        /// <summary>
        /// Obtiene una entidad con el predicado dado o nulo si no la encuentra.
        /// </summary>
        /// <param name="predicate">Predicado para filtrar las entidades</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Crea una entidad con la clave primaria dada sin accesar la base de datos.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad a cargar</param>
        /// <returns>Entity</returns>
        TEntity Load(TType id);

        #endregion

        #region Insert

        /// <summary>
        /// Inserta una nueva entidad.
        /// </summary>
        /// <param name="entity">Entidad insertada</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserta una nueva entidad.
        /// </summary>
        /// <param name="entity">Entidad insertada</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserta una nueva entidad y obtiene su Id
        /// Puede necesitar que se guarde la unidad de trabajo actal
        /// para poder recuperar el id
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Id de la entidad</returns>
        TType InsertAndGetId(TEntity entity);

        /// <summary>
        /// Inserta una nueva entidad y obtiene su Id
        /// Puede necesitar que se guarde la unidad de trabajo actal
        /// para poder recuperar el id
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Id de la entidad</returns>
        Task<TType> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// Inserta o actualiza la entidad dada dependiendo del valor de su Id.
        /// </summary>
        /// <param name="entity">Entidad</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Inserta o actualiza la entidad dada dependiendo del valor de su Id.
        /// </summary>
        /// <param name="entity">Entidad</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// Inserta o actualiza la entidad dada dependiendo del valor de su Id.
        /// También regresa el Id de la entidad
        /// Puede necesitar que se guarde la unidad de trabajo actal
        /// para poder recuperar el id
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Id de la entidad</returns>
        TType InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// Inserta o actualiza la entidad dada dependiendo del valor de su Id.
        /// También regresa el Id de la entidad
        /// Puede necesitar que se guarde la unidad de trabajo actal
        /// para poder recuperar el id
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Id de la entidad</returns>
        Task<TType> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion

        #region Update

        /// <summary>
        /// Actualiza un entidad existente.
        /// </summary>
        /// <param name="entity">Entidad</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Actualiza un entidad existente.
        /// </summary>
        /// <param name="id">Id de la entidad</param>
        /// <param name="updateAction">Acción que puede ser usada para cambiar los valores de la entidad</param>
        /// <returns>Entidad actualizada</returns>
        TEntity Update(TType id, Action<TEntity> updateAction);

        /// <summary>
        /// Actualiza un entidad existente. 
        /// </summary>
        /// <param name="entity">Entidad</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Actualiza un entidad existente.
        /// </summary>
        /// <param name="id">Id de la entidad</param>
        /// <param name="updateAction">Acción que puede ser usada para cambiar los valores de la entidad</param>
        /// <returns>Entidad actualizada</returns>
        Task<TEntity> UpdateAsync(TType id, Func<TEntity, Task> updateAction);

        #endregion

        #region Delete

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entity">Entidad a ser eliminada</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Elimina una entidad por su clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad</param>
        void Delete(TType id);

        /// <summary>
        /// Elimina muchas entidades por función.
        /// Nota: Todas las entidades que cumplan con las condiciones son recuperadas y eliminadas.
        /// Esto puede causar problemas de rendimiento mayores si hay demasiadas entidades con
        /// el predicado dado.
        /// </summary>
        /// <param name="predicate">Una condición para filtrar entidades</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entity">Entidad a ser eliminada</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Elimina una entidad por su clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad</param>
        Task DeleteAsync(TType id);

        /// <summary>
        /// Elimina muchas entidades por función.
        /// Nota: Todas las entidades que cumplan con las condiciones son recuperadas y eliminadas.
        /// Esto puede causar problemas de rendimiento mayores si hay demasiadas entidades con
        /// el predicado dado.
        /// </summary>
        /// <param name="predicate">Una condición para filtrar entidades</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Aggregates

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio
        /// </summary>
        /// <returns>Cantidad de entidades</returns>
        int Count();

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio basado en el <paramref name="predicate"/> dado.
        /// </summary>
        /// <param name="predicate">Un método para filtrar el conteo</param>
        /// <returns>Cantidad de entidades</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio
        /// </summary>
        /// <returns>Cantidad de entidades</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio basado en el <paramref name="predicate"/> dado.
        /// </summary>
        /// <param name="predicate">Un método para filtrar el conteo</param>
        /// <returns>Cantidad de entidades</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio (usalo si esperas un valor más grande que <see cref="int.MaxValue"/>).
        /// </summary>
        /// <returns>Cantidad de entidades</returns>
        long LongCount();

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio basado en el <paramref name="predicate"/> dado
        /// (usa esta sobrecarga si esperas un valor más grande que <see cref="int.MaxValue"/>).
        /// </summary>
        /// <param name="predicate">Un método para filtrar el conteo</param>
        /// <returns>Cantidad de entidades</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio (usalo si esperas un valor más grande que <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>Cantidad de entidades</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// Obtiene el conteo de todas las cantidades en este repositorio basado en el <paramref name="predicate"/> dado
        /// (usa esta sobrecarga si esperas un valor más grande que <see cref="int.MaxValue"/>).
        /// </summary>
        /// <param name="predicate">Un método para filtrar el conteo</param>
        /// <returns>Cantidad de entidades</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        string GetTableName();
        List<string> GetColumnNames();
    }
}
