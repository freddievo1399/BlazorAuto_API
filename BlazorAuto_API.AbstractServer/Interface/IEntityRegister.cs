﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuto_API.AbstractServer
{
    public interface IEntityRegister
    {
        void RegisterEntities(ModelBuilder modelbuilder);
    }
}
