using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure
{
    public class PermistionAttribute : Attribute
    {
        public string NameVi { get; set; }

        public PermistionAttribute(string nameVN)
        {
            NameVi = nameVN;
        }
    }
}
