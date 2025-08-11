using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlazorAuto_API.Abstract
{
    public static class HttpUltil
    {
        public static async Task<Result> GetResultAsync(this HttpResponseMessage response)
        {
            var resp = await response.Content.ReadAsStringAsync();
            var rsl= JsonConvert.DeserializeObject<Result>(resp);
            return rsl??Result.Error(resp);
        }
    }
}
