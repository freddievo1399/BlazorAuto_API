﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public interface IExcuteService<T>
    {

        public V Excute<V>(Func<T, V> func);
    }
}
