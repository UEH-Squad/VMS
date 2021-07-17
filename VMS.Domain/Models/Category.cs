namespace VMS.Domain.Models
{
    public class Category : DeleteEntity<int>
    {
        public string Name { get; set; }
    }
}