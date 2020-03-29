using System;

namespace Covida.Core.Definition
{
    public interface IDomain
    {
        DateTime CreatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }

    public static class IDomainExtensions
    {
        public static bool IsDeleted(this IDomain @this) => @this.DeletedAt.HasValue;
    }
}
