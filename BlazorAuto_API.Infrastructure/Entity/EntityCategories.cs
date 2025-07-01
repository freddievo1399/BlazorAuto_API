using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure;
[Table("Category")]
public class EntityCategory : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(255)]
    public string? Description { get; set; }

    public ICollection<EntityProductCategory> ProductCategories { get; set; } = new List<EntityProductCategory>();


    [NotMapped]
    public ICollection<EntityProduct> Product
    {
        get => ProductCategories.Select(x => x.Product).ToList() ?? new();
    }
}
