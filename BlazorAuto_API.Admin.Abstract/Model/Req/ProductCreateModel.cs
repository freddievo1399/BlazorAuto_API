using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductCreateModel
    {
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Mô tả sản phẩm")]
        public string? Description { get; set; }
    }
}
