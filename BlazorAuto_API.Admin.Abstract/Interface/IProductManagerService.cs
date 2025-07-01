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
    [BasePath("/api/admin/productManager")]
    public interface IProductManagerService
    {
        [Get(nameof(FindByGuid))]
        Task<ResultOf<ProductInfoModel>> FindByGuid(Guid Guid);


        [Post(nameof(Remove))]
        Task<Result> Remove(Guid Guid);

        [Post(nameof(GetData))]
        Task<PagedResultsOf<ProductInfoModel>> GetData([Body] DataRequestDto Request);

        [Post(nameof(Add))]
        Task<Result> Add([Body] ProductCreateModel Request);

    }
}
