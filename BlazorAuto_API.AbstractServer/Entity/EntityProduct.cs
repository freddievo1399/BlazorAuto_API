﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.AbstractServer
{
    [Table("Product")]
    public class EntityProduct : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }
        public ICollection<ProductSpecification> Specifications { get; set; }

        public ICollection<EntityProductCategory> ProductCategories { get; set; }

        [NotMapped]
        public ICollection<EntityCategory> Categories
        {
            get => ProductCategories.Select(x => x.Category).ToList();
        }
    }

}
