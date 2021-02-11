using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class UserProfile : Entity
    {
        [MaxLength(50)]
        public string Avatar { get; set; }

        [MaxLength(50)]
        public string Nick { get; set; }

        [Required]
        [ForeignKey(nameof(Id))]
        public User User { get; set; }
    }
}
