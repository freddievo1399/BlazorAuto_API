using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public class FeatureAttribute: Attribute
    {
        public string GroupName { get; set; }
        //public string Name { get; set; }
        public string NameVN { get; set; }

        public FeatureAttribute(string groupId, string nameVN)
        {
            GroupName = groupId;
            NameVN = nameVN;
        }
    }
}
