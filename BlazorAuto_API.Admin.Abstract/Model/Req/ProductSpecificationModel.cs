using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductSpecificationModel
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage ="Nhập tên")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage ="Nhập giá")]
        public decimal Price { get; set; }

        public decimal? DicountPrice { get; set; }

        public string? Description { get; set; }

        public bool IsEnable { get; set; } = true;
    }
}
