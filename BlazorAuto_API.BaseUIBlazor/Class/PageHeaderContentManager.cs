using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorAuto_API.BaseUIBlazor
{
    public class PageHeaderContentManager
    {
        public RenderFragment Content { get; private set; }

        public void Set(RenderFragment content)
        {
            Content = content;
        }
    }

}
