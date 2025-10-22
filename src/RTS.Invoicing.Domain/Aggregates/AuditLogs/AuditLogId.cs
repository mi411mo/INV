namespace RTS.Invoicing.Domain.Aggregates.AuditLogs
{
    /// <summary>
    /// Represents the unique identifier for an audit log entry.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public sealed record AuditLogId(long Value);
}
