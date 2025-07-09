using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace BlazorAuto_API.Infrastructure;

[Table("Product")]
public class EntityProduct : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? RichDescription { get; set; }

    public ICollection<EntityProductSpecification> Specifications { get; set; } = new List<EntityProductSpecification>();

    public ICollection<EntityProductCategory> ProductCategories { get; set; } = new List<EntityProductCategory>();
    //[JsonPropertyName("ImageJson")]
    //[JsonData]
    //[NotMapped]
    public ICollection<ImageInfo> Images { get; set; } = new List<ImageInfo>();
    public ICollection<ImageInfo> CarouselImages { get; set; } = new List<ImageInfo>();

    [NotMapped]
    public ICollection<EntityCategory> Categories
    {
        get => ProductCategories?.Select(x => x.Category).ToList() ?? new List<EntityCategory>();
    }
}
