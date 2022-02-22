using System;
using System.Collections.Generic;

namespace BookStoresWebAPI.Models
{
    public partial class Job
    {
        public Job()
        {
            Users = new HashSet<User>();
        }

        public int JobId { get; set; }
        public string JobDesc { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
