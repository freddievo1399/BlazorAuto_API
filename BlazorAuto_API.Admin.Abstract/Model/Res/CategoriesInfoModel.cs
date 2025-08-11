using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class CategoriesInfoModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = default!;

        [MaxLength(255)]
        public string? Description { get; set; }
    }
}
