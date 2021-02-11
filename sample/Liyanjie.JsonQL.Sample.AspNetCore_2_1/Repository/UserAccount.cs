﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liyanjie.JsonQL.Sample.AspNetCore_2_1
{
    public class UserAccount : Entity
    {
        public decimal Coins { get; set; }

        public decimal Points { get; set; }

        [Required]
        [ForeignKey(nameof(Id))]
        public User User { get; set; }
    }
}
