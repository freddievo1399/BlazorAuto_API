using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure
{
    [Table("Product_Category")]
    public class EntityProductCategory : EntityBase
    {
        [Required]
        public int ProductId { get; set; }
        [NotMapped]
        public EntityProduct Product { get; set; } = default!;

        [Required]
        public int CategoryId { get; set; }
        [NotMapped]
        public EntityCategory Category { get; set; } = default!;
    }

}
