﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Abstract
{
    public interface ILoadAssemlyBlazor
    {
        public List<Assembly> LoadAssemblyAsync();
    }
}
