using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductSpecificationInfoModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; } = default!;

        public decimal? Price { get; set; }

        public decimal? DicountPrice { get; set; }

        public string? Description { get; set; }

        public bool IsEnable { get; set; }
    }
}
