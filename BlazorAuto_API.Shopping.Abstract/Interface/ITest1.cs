using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RestEase;
using Syncfusion.Blazor;
namespace BlazorAuto_API.Shopping.Abstract;

[BasePath("/api/base/Test1")]
public interface ITest1
{
    [Get(nameof(ReturnString))]
    Task<string> ReturnString(int input);

    [Post(nameof(TestQuery))]
    Task<string> TestQuery([Body] DataManagerRequest input);
}
