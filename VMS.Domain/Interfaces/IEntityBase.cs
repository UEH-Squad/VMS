namespace VMS.Domain.Interfaces
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }
}