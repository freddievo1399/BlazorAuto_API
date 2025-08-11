using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ReqDeleteImages
    {
        public Guid Guid { get; set; }
        [Required(ErrorMessage = "Chọn ít nhất một ảnh để xóa")]
        public List<Guid>? GuidImage { get; set; }
    }
}
