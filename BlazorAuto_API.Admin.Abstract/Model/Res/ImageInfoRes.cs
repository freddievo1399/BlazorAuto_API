using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ImageInfoRes
    {
        public Guid Guid { get; set; }
        [Required(ErrorMessage = "Nhập đường dẫn ảnh")]
        public string Src { get; set; } = "";
        [Required(ErrorMessage = "Nhập tên ảnh")]
        public string? FileName { get; set; }

        public bool Selected { get; set; }
    }
}
