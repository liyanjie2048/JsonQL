using System;
using System.ComponentModel.DataAnnotations;

namespace Liyanjie.JsonQL.Sample.AspNetCore_2_1
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
