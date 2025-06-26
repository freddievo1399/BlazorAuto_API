using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class CategoriesModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = default!;

        [MaxLength(255)]
        public string? Description { get; set; }

        public List<string> ProductNames { get; set; } = new List<string>();
    }
}
