using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure
{
    public abstract class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [NonDuplicatedAttribute]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = default!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        [NotMapped]
        public bool IsDeleted => !DeletedAt.HasValue;
    }


}
