using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Infrastructure.Attributes;
using Newtonsoft.Json.Linq;

namespace BlazorAuto_API.Infrastructure
{
    public static class GlobalPermistion
    {
        public static List<FeaturesModel> FeaturesModels { get; } = [];
        private static void AddFeature(FeaturesModel featuresModel) => FeaturesModels.Add(featuresModel);
        public static void Register<TEnum>() where TEnum : Enum
        {
            var typeEnum = typeof(TEnum);
            Type declaringClass = typeEnum.DeclaringType!;
            var Features = typeEnum.GetCustomAttribute<FeatureAttribute>() ?? throw new Exception("Chưa cấu hình Feature " + typeEnum.Name);
            FeaturesModel featuresModel = new (declaringClass.Name, Features.NameVN);
            foreach (var function in Enum.GetValues(typeEnum))
            {
                var functionName = function.ToString();
                var member = typeEnum.GetMember(functionName!).FirstOrDefault();
                if (member != null)
                {
                    var functionAttr = member.GetCustomAttribute<PermistionAttribute>() ?? throw new Exception($"Chưa cấu hình Permistion {typeEnum.Name}.{functionName}");
                    featuresModel.permistionModels.Add(new() { Name = functionName!, Value = (int)Math.Pow(2, (int)function), NameVi = functionAttr.NameVi });
                }
            }
            AddFeature(featuresModel);

        }
    }
}
