using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Admin.Abstract
{
    [BasePath("/api/admin/Categories")]
    public interface ICategoriesService
    {
        [Get(nameof(FindByGuid))]
        Task<ResultOf<CategoriesModel>> FindByGuid(Guid Guid);

        [Post(nameof(GetData))]
        Task<PagedResultsOf<CategoriesModel>> GetData([Body] DataManagerRequest input);
        
    }
}
