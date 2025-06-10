using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RestEase;
namespace BlazorAuto_API.Shopping.Abstract
{
    [BasePath("/api/base/Test1")]
    public interface ITest1
    {
        [Get(nameof(returnString))]
        Task<string> returnString(int input);
    }
}
