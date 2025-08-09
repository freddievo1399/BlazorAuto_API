using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;

namespace BlazorAuto_API.Admin.Abstract
{
    [BasePath("/api/admin/ManagerRole")]

    public interface IManagerRole
    {
        [Get(nameof(GetData))]
        Task<ResultsOf<Role>> GetData();

        [Get(nameof(GetUsers))]
        Task<ResultsOf<string>> GetUsers();

        [Post(nameof(UpdateRole))]
        Task<Result> UpdateRole([Body] List<Role> roles);
    }
}
