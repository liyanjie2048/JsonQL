using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Liyanjie.JsonQL.Sample.AspNetCore_2_1
{
    public class Order : Entity
    {
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;

        public string Serial { get; set; } = DateTimeOffset.Now.ToString("yyyyMMddHHmmssffffff");

        public int Status { get; set; }

        [Required]
        public User User { get; set; }

        public ICollection<OrderStatusChange> StatusChanges { get; set; }
    }
}
