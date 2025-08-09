using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Admin.Abstract
{
    public class Role
    {
        public string RoleName { get; set; }
        public List<string> UserNames { get; set; } = new();
        public List<Permission> Permissions { get; set; } = new();
    }
}
