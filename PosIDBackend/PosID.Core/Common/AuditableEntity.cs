using System;

namespace PosID.Core.Common
{
    public abstract class AuditableEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
    }
}
