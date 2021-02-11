using System;
using System.ComponentModel.DataAnnotations;

namespace Liyanjie.JsonQL.Sample.AspNetCore_2_1
{
    public class UserAccountRecord : Entity
    {
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;

        public decimal Coins { get; set; }

        public decimal Points { get; set; }

        [MaxLength(50)]
        public string Remark { get; set; }

        [Required]
        public User User { get; set; }
    }
}
