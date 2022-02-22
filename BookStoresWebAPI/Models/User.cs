using System;
using System.Collections.Generic;

namespace BookStoresWebAPI.Models
{
    public partial class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int UserId { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int? JobId { get; set; }
        public int? JobLevel { get; set; }
        public int PubId { get; set; }
        public DateTime? HireDate { get; set; }

        public virtual Job? Job { get; set; }
        public virtual Publisher Pub { get; set; } = null!;
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
