using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductUpdateModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string RichDescription { get; set; }

        public ICollection<Guid>? CategoryGuids { get; set; }
    }
}
