using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.AuditLogs
{
    /// <summary>
    /// Represents a single record of an auditable change, creation, or deletion event within the system.
    /// This entity tracks who performed an action, on what entity, and what the data changed from/to.
    /// </summary>
    public sealed class AuditLog : Entity<AuditLogId>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="AuditLog"/> class from being created.
        /// <summary>
        /// Parameterless constructor used by ORMs to materialize AuditLog instances.
        /// </summary>
        private AuditLog()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLog"/> class with all necessary audit information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="action">The action.</param>
        /// <param name="oldValues">The old values.</param>
        /// <summary>
        /// Initializes a new AuditLog instance representing an auditable change or event.
        /// </summary>
        /// <param name="id">Unique identifier for the audit log entry.</param>
        /// <param name="userId">Identifier of the user who performed the action, or null for system-initiated actions.</param>
        /// <param name="timeStamp">Timestamp when the action occurred.</param>
        /// <param name="entityId">Identifier of the affected entity as a string.</param>
        /// <param name="entityName">Type name of the affected entity (for example, "Invoice" or "Customer").</param>
        /// <param name="action">Action performed on the entity (for example, "Create", "Update", "Delete").</param>
        /// <param name="oldValues">Serialized representation of the entity state before the action, or null if not applicable.</param>
        /// <param name="newValues">Serialized representation of the entity state after the action, or null if not applicable.</param>
        public AuditLog(
            AuditLogId id,
            long? userId,
            DateTime timeStamp,
            string entityId,
            string entityName,
            string action,
            string? oldValues,
            string? newValues)
            : base(id)
        {
            UserId = userId;
            TimeStamp = timeStamp;
            EntityId = entityId;
            EntityName = entityName;
            Action = action;
            OldValues = oldValues;
            NewValues = newValues;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the user who initiated the change.
        /// It is nullable because some system actions may not be tied to a specific user.
        /// </summary>
        public long? UserId { private set; get; }

        /// <summary>
        /// Gets or sets the date and time when the auditable action occurred.
        /// </summary>
        public DateTime TimeStamp { private set; get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the string representation of the primary key (ID) of the entity that was affected by the action.
        /// </summary>
        public string EntityId { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the type name of the affected entity (e.g., 'Invoice', 'Customer').
        /// </summary>
        public string EntityName { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the action performed on the entity (e.g., Create, Update, Delete).
        /// </summary>
        public string Action { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the serialized data representing the state of the entity *before* the action occurred (for updates/deletes).
        /// </summary>
        public string? OldValues { private set; get; }

        /// <summary>
        /// Gets or sets the serialized data representing the state of the entity *after* the action occurred (for creates/updates).
        /// </summary>
        public string? NewValues { private set; get; }
    }
}