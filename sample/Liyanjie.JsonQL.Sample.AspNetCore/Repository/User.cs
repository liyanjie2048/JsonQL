using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liyanjie.JsonQL.Sample.AspNetCore
{
    public class User : Entity
    {
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public UserProfile Profile { get; set; }

        [Required]
        public UserAccount Account { get; set; }

        public ICollection<UserAccountRecord> AccountRecords { get; set; } = new List<UserAccountRecord>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
