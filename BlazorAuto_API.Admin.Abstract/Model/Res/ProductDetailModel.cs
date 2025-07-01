using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductDetailModel : ProductInfoModel
    {
        public string? RichDescription { get; set; }

        public ICollection<Guid> CategoryGuids { get; set; }
    }
}
