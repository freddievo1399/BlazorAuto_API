using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public class PermistionModel
    {
        public string Name { get; set; } = "";
        public string NameVi { get; set; } = "";
        public int Value { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
