using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorAuto_API.Infrastructure
{
    public static class MutablePropertyUltil
    {
        public static bool HasAnnotation<T>(this IMutableProperty property)
        {
            var nameProperty = typeof(T).Name;
            if (nameProperty.EndsWith("Attribute"))
            {
                var temp = nameProperty.Split("Attribute").ToList();
                temp.RemoveAt(temp.Count - 1);
                nameProperty = string.Concat(temp);
            }
            var rlt = property.FindAnnotation(nameProperty);

            return rlt != null;
        }
        public static IAnnotation? FindAnnotation<T>(this IMutableProperty property)
        {
            var nameProperty = typeof(T).Name;
            if (nameProperty.EndsWith("Attribute"))
            {
                var temp = nameProperty.Split("Attribute").ToList();
                temp.RemoveAt(temp.Count - 1);
                nameProperty = string.Concat(temp);
            }

            return property.FindAnnotation("nameProperty"); ;
        }
    }
}
