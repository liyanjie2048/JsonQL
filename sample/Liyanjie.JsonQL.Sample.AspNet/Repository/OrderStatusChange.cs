using System;
using System.ComponentModel.DataAnnotations;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class OrderStatusChange : Entity
    {
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public int Status { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
