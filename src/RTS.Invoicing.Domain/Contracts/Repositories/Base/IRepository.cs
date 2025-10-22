using RTS.Invoicing.Domain.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Invoicing.Domain.Contracts.Repositories.Base
{
    /// <summary>
    /// Defines a standard contract for basic data access operations (CRUD) for a specific type of Domain Entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the domain entity (which typically inherits from <c>Entity</c>) that the repository manages.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Retrieves a single entity asynchronously based on its unique primary identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity (of type long).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> returning a <see cref="Result{T}"/> containing the entity on success, or an error on failure (e.g., Not Found).</returns>
        public Task<Result<TEntity>> GetByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a collection of entities asynchronously, often applying filtering logic based on the provided entity instance (used as a specification or criteria).
        /// </summary>
        /// <param name="entity">An instance of the entity type used to define specific query criteria or filters (e.g., retrieving all InvoiceItems associated with a specific Invoice).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> returning a <see cref="Result{T}"/> containing a collection of entities on success.</returns>
        public Task<Result<IEnumerable<TEntity>>> GetAllAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Persists a new entity asynchronously to the data store.
        /// </summary>
        /// <param name="entity">The entity instance to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> returning a parameterless <see cref="Result"/> indicating success or failure of the insertion.</returns>
        public Task<Result> AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the state of an existing entity asynchronously in the data store.
        /// </summary>
        /// <param name="entity">The entity instance containing the updated data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> returning a parameterless <see cref="Result"/> indicating success or failure of the update operation.</returns>
        public Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified entity asynchronously from the data store.
        /// </summary>
        /// <param name="entity">The entity instance to be deleted.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> returning a parameterless <see cref="Result"/> indicating success or failure of the deletion operation.</returns>
        public Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
