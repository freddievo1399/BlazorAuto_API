using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductUpdateModel
    {
        public Guid Guid { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? RichDescription { get; set; }

        public ICollection<Guid>? CategoryGuids { get; set; }
        public ICollection<ProductSpecificationModel>? ProductSpecifications { get; set; }

        public static implicit operator ProductUpdateModel(ProductDetailModel model)
        {
            return new()
            {
                CategoryGuids = model.CategoryGuids,
                Description = model.Description,
                Guid = model.Guid,
                Name = model.Name,
                ProductSpecifications = model.ProductSpecifications.ToList(),
                RichDescription = model.RichDescription,                
            };
        }

    }
}
