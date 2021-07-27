using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}