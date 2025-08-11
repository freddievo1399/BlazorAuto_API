using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace BlazorAuto_API.Admin.Abstract
{
    public class ProductDetailModel : ProductInfoModel
    {
        public string? RichDescription { get; set; }

        public List<Guid> CategoryGuids { get; set; } = new();
        public List<ImageInfoRes> ImageInfos { get; set; }= new();
        public List<ImageInfoRes> CarouselImages { get; set; } = new();

        public List<ProductSpecificationModel>? ProductSpecifications { get; set; }

    }
}
