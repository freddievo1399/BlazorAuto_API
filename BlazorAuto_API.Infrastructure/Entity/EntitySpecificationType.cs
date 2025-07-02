using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure;
[Table("Product_Specification")]
public class EntityProductSpecification : EntityBase
{
    [Required]
    public Guid ProductGuid { get; set; }

    public EntityProduct Product { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? DicountPrice { get; set; }

    public string? Description { get; set; }

    public bool IsEnable { get; set; }
}

