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
        public RenderFragment Content { get; private set; } = _ => { };

        public void Set(RenderFragment? content)
        {
            if (content == null)
            {
                Content = _ => { };
                return;
            }
            Content = content;
        }
    }

}
