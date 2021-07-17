using VMS.Domain.Interfaces;

namespace VMS.Domain.Models
{
    public abstract class DeleteEntity<TKey> : EntityBase<TKey>, IDeleteEntity<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}