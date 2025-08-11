using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorAuto_API.Abstract
{
    public static class ListFileUltil
    {
        public static MultipartFormDataContent GetMultipartFormDataContent(this List<IBrowserFile> files, string NameFild, long maxSize = long.MaxValue)
        {
            var multipartContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                multipartContent.Add(new StreamContent(file.OpenReadStream(maxSize)), NameFild,file.Name);
            }
            return multipartContent;
        }
    }
}
