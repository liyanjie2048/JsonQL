using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class User : Entity
    {
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [ForeignKey(nameof(Id))]
        public UserProfile Profile { get; set; }

        [ForeignKey(nameof(Id))]
        public UserAccount Account { get; set; }

        public ICollection<UserAccountRecord> AccountRecords { get; set; } = new List<UserAccountRecord>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
