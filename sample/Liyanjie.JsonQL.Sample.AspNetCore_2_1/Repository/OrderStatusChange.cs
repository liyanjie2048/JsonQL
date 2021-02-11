﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Liyanjie.JsonQL.Sample.AspNetCore_2_1
{
    public class OrderStatusChange : Entity
    {
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;

        public int Status { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
