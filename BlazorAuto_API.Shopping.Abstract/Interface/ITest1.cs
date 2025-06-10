using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Shopping.Abstract
{
    public interface ITest1
    {
        Task<string> returnString(int input);
    }
}
