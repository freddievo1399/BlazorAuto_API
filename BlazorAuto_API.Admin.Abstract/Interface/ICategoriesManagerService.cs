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
    [BasePath("/api/admin/CategoriesManager")]
    public interface ICategoriesManagerService
    {
        [Get(nameof(FindByGuid))]
        Task<ResultOf<CategoriesInfoModel>> FindByGuid(Guid Guid);

        [Post(nameof(Remove))]
        Task<Result> Remove(Guid Guid);

        [Post(nameof(GetData))]
        Task<PagedResultsOf<CategoriesInfoModel>> GetData([Body] DataRequestDto Request);

        [Post(nameof(Add))]
        Task<Result> Add([Body] CategoriesCreateModel Request);

    }
}
