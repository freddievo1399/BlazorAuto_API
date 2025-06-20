using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.AbstractServer;
[Table("Product_Specification")]
public class ProductSpecification : EntityBase
{
    public int ProductId { get; set; }
    public EntityProduct Product { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string Value { get; set; } = default!;
}

