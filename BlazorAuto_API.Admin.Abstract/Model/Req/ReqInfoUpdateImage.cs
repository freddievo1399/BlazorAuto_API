using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ReqInfoUpdateImage
    {
        [Required]
        public Guid? Guid { get; set; }

        [Required]
        public Guid? GuidImage { get; set; }
    }
}
