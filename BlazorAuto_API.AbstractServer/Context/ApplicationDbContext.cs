using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuto_API.AbstractServer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityRegisters = AssembliesUtil.GetAssemblies().GetInstances<IEntityRegister>();

            foreach (var i in entityRegisters)
            {
                i.RegisterEntities(modelBuilder);
            }
            //foreach (var modelType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var nameTable = modelType.FindAnnotation("Relational:TableName");
            //    if (nameTable != null)
            //    {
            //        var entity = modelBuilder.Entity(modelType.Name);
            //        entity.ToTable(nameTable.Value?.ToString());
            //        foreach (var prop in entity.Metadata.GetDeclaredProperties())
            //        {

            //            var FullAnotation = prop.PropertyInfo.GetCustomAttribute<NvarcharAttribute>();
            //            if (prop.Name == "Description")
            //            {
            //            }
            //            var MaxLengthAnnotation = prop.FindAnnotation("MaxLength");
            //            if (MaxLengthAnnotation == null)
            //            {
            //                entity.Property(prop.Name).HasMaxLength(100);
            //            }
            //            if (prop.ClrType == typeof(decimal) || prop.ClrType == typeof(decimal?))
            //            {
            //                var Precision = prop.FindAnnotation("Precision");

            //                if (Precision == null)
            //                {
            //                    entity.Property(prop.Name).HasPrecision(18, 6);
            //                }
            //                continue;
            //            }
            //            var NVarCharAnnotion = prop.FindAnnotation("NvarcharAttribute");
            //            var JsonDataAttribute = prop.FindAnnotation("JsonDataAttribute");
            //            if (NVarCharAnnotion != null)
            //            {
            //                entity.Property(prop.Name).IsUnicode();
            //            }
            //            if (JsonDataAttribute != null)
            //            {
            //                entity.Property(prop.Name).HasColumnType("nvarchar(max)").HasJsonPropertyName("JsData" + prop.Name);
            //            }
            //        }

            //    }
            //}
            base.OnModelCreating(modelBuilder);
        }
    }
}
