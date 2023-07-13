using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Repositories
{
    /// <summary>
    /// Represents the base collection of all BaseEntity entities in the repository's context.
    /// </summary>
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Interface method for synchronously getting all TEntity with order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>A collection of all queried TEntity</returns>
        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Interface method for asynchronously getting all TEntity with order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>An awaitable collection of all queried TEntity</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Interface method for synchronously getting all TEntity with filter by, order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Property to filter by</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>A collection of all queried TEntity</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Interface method for asynchronously getting all TEntity with filter by, order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>An awaitable collection of all queried TEntity</returns>
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Interface method for synchronously getting a single record of TEntity using filter and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>The queried TEntity</returns>
        TEntity GetOne(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null);

        /// <summary>
        /// Interface method for asynchronously getting a single record of TEntity using filter and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>An awaitable queried TEntity</returns>
        Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null);

        /// <summary>
        /// Interface method for synchronously getting the first record in a collection of TEntity using filter, order by, and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>The first record in the collection of the queried TEntity</returns>
        TEntity GetFirst(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Interface method for asynchronously getting the first record in a collection of TEntity using filter, order by, and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>An awaitable of the first record in the collection of the queried TEntity</returns>
        Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Interface method for synchronously getting a TEntity by its id with include query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the TEntity</param>
        /// <param name="includeProperties">Child entity to be included</param>
        /// <returns>The queried TEntity</returns>
        TEntity GetById(object id, string includeProperties = null);

        /// <summary>
        /// Interface method for asynchronously getting a TEntity by its id with include query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the TEntity</param>
        /// <param name="includeProperties">Child entity to be included</param>
        /// <returns>An awaitable queried TEntity</returns>
        Task<TEntity> GetByIdAsync(object id, string includeProperties = null);

        /// <summary>
        /// Interface method for synchronously getting the count of all TEntity with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>The count of all queried records.</returns>
        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Interface method for asynchronously getting the count of all TEntity with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>An awaitable count of all queried records.</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Interface method for synchronously check if TEntity exists in the context with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>A boolean value indicates whether the TEntity exists in the context.</returns>
        bool GetExists(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Interface method for asynchronously check if TEntity exists in the context with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>An awaitable boolean value indicates whether the TEntity exists in the context.</returns>
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Interface method for creating new TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the newly created entity.</returns>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Interface method for updating an existing TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the updated entity.</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Interface method for deleting a TEntity by its id
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the entity</param>
        /// <returns>An awaitable of the deleted entity.</returns>
        Task<TEntity> Delete(object id);

        /// <summary>
        /// Interface method for deleting a TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the deleted entity.</returns>
        Task<TEntity> Delete(TEntity entity);

        /// <summary>
        /// Interface method for synchronously saving all changes in the context.
        /// </summary>
        void Save();

        /// <summary>
        /// Interface method for asynchronously saving all changes in the context.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }

    /// <summary>
    /// Represents a base repository of <see cref="BaseEntity"/> entities
    /// </summary>
    /// <typeparam name="TContext">The subtype of <see cref="USFWritesDbContext"/> that provides the backing store for the repository.</typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Helper method for getting all TEntity with filter, order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Property to filter by</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>A collection of all queried TEntity</returns>
        protected virtual IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else // Order by Id by default because Skip requires OrderBy
            {
                query = query.OrderBy(e => e.Id);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        /// <summary>
        /// Implementation method for synchronously getting all TEntity with order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>A collection of all queried TEntity</returns>
        public virtual IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            return GetQueryable(null, orderBy, includeProperties, skip, take).ToList();
        }

        /// <summary>
        /// Implementation method for asynchronously getting all TEntity with order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>An awaitable collection of all queried TEntity</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        {
            return await GetQueryable(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        /// <summary>
        /// Imeplementation method for synchronously getting all TEntity with filter by, order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Property to filter by</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>A collection of all queried TEntity</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take).ToList();
        }

        /// <summary>
        /// Implementation method for asynchronously getting all TEntity with filter by, order by, include, skip, and take queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <param name="skip">Number of skipped records</param>
        /// <param name="take">Number of records to take</param>
        /// <returns>An awaitable collection of all queried TEntity</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        /// <summary>
        /// Implementation method for synchronously getting a single record of TEntity using filter and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>The queried TEntity</returns>
        public virtual TEntity GetOne(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefault();
        }

        /// <summary>
        /// Implementation method for asynchronously getting a single record of TEntity using filter and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>An awaitable queried TEntity</returns>
        public virtual async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
        {
            return await GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Implementation method for synchronously getting the first record in a collection of TEntity using filter, order by, and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>The first record in the collection of the queried TEntity</returns>
        public virtual TEntity GetFirst(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = null)
        {
            return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
        }

        /// <summary>
        /// Implementation method for asynchronously getting the first record in a collection of TEntity using filter, order by, and include queries.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <param name="orderBy">Property to order by</param>
        /// <param name="includeProperties">Child Entity to be included</param>
        /// <returns>An awaitable of the first record in the collection of the queried TEntity</returns>
        public virtual async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Implementation method for synchronously getting a TEntity by its id with include query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the TEntity</param>
        /// <param name="includeProperties">Child entity to be included</param>
        /// <returns>The queried TEntity</returns>
        public virtual TEntity GetById(object id, string includeProperties = null)
        {
            try
            {
                Expression<Func<TEntity, bool>> filter = e => e.Id == (int)id;
                return GetQueryable(filter, null, includeProperties).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Implementation method for asynchronously getting a TEntity by its id with include query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the TEntity</param>
        /// <param name="includeProperties">Child entity to be included</param>
        /// <returns>An awaitable queried TEntity</returns>
        public virtual Task<TEntity> GetByIdAsync(object id, string includeProperties = null)
        {
            try
            {
                Expression<Func<TEntity, bool>> filter = e => e.Id == (int)id;
                return GetQueryable(filter, null, includeProperties).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Implementation method for synchronously getting the count of all TEntity with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>The count of all queried records.</returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        /// <summary>
        /// Implementation method for asynchronously getting the count of all TEntity with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>An awaitable count of all queried records.</returns>
        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        /// <summary>
        /// Implementation method for synchronously check if TEntity exists in the context with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>A boolean value indicates whether the TEntity exists in the context.</returns>
        public virtual bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        /// <summary>
        /// Implementation method for asynchronously check if TEntity exists in the context with filter query.
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="filter">Filter by query</param>
        /// <returns>An awaitable boolean value indicates whether the TEntity exists in the context.</returns>
        public virtual Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).AnyAsync();
        }

        /// <summary>
        /// Implementation method for creating new TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the newly created entity.</returns>
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Implementation method for updating an existing TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the updated entity.</returns>
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Implementation method for deleting a TEntity by its id
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="id">Id of the entity</param>
        /// <returns>An awaitable of the deleted entity.</returns>
        public virtual async Task<TEntity> Delete(object id)
        {
            TEntity entity = GetById(id);
            await Delete(entity);
            return entity;
        }

        /// <summary>
        /// Implementation method for deleting a TEntity
        /// </summary>
        /// <typeparam name="TEntity">Generic-typed entity</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>An awaitable of the deleted entity.</returns>
        public virtual async Task<TEntity> Delete(TEntity entity)
        {
            if (entity == null)
            {
                return entity;
            }

            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Implementation method for synchronously saving all changes in the context.
        /// </summary>
        public virtual void Save()
        {
            var entities = from e in context.ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Interface method for asynchronously saving all changes in the context.
        /// </summary>
        /// <returns></returns>
        public virtual Task SaveAsync()
        {
            var entities = from e in context.ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return context.SaveChangesAsync();
            //return Task.FromResult(0);
        }

        /// <summary>
        /// Helper method for exception handling
        /// </summary>
        /// <param name="e">DbEntityValidationException</param>
        //protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        //{
        //    var errorMessages = e.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => x.ErrorMessage);

        //    var fullErrorMessage = string.Join("; ", errorMessages);
        //    var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
        //    throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        //}

        public DataTable ExecRawSelect(string command)
        {
            var tb = new DataTable();
            using (var con = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                var a = new SqlDataAdapter(command, con);
                a.Fill(tb);
            }

            return tb;
        }
    }
}
