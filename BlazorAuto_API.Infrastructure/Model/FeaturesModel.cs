using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure
{
    public class FeaturesModel
    {
        public string Name { get; set; }
        public string NameVi { get; set; }
        public List<PermistionModel> permistionModels { get; set; } = new List<PermistionModel>();

        public FeaturesModel(string name, string nameVi)
        {
            Name = name;
            NameVi = nameVi;
        }
    }
}
