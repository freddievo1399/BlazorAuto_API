using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public class FeaturesModel
    {
        public string Name { get; set; }
        public string NameVi { get; set; }
        public string Group { get; set; }
        public List<PermistionModel> permistionModels { get; set; } = new List<PermistionModel>();

        public FeaturesModel(string groupId, string name, string nameVi)
        {
            Group = groupId;
            Name = name;
            NameVi = nameVi;
        }
    }
}
