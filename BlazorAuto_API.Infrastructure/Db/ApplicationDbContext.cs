using System.Threading.Tasks;
using Azure.Core;
using BlazorAuto_API.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityRegisters = AssembliesUtil.GetAssemblies().GetInstances<IEntityRegister>();
            EntityRegister entityRegisterBase = new();
            entityRegisterBase.RegisterEntities(modelBuilder);
            foreach (var i in entityRegisters)
            {
                i.RegisterEntities(modelBuilder);
            }
            foreach (var modelType in modelBuilder.Model.GetEntityTypes())
            {
                var nameTable = modelType.FindAnnotation("Relational:TableName");
                var entity = modelBuilder.Entity(modelType.Name);
                if (nameTable != null)
                {
                    entity.ToTable(nameTable.Value?.ToString());
                }
                entity.HasIndex("Guid").IsUnique();

                var props = entity.Metadata.GetDeclaredProperties();
                foreach (IMutableProperty prop in props)
                {
                    var propEntity = entity.Property(prop.Name);
                    var clrType = prop.ClrType;
                    if (clrType == typeof(string))
                    {
                        var MaxLength = prop.FindAnnotation<PrecisionAttribute>();
                        if (MaxLength != null)
                        {
                            propEntity.HasMaxLength(100);
                            continue;
                        }

                        var VarCharAnnotion = prop.FindAnnotation<NvarcharAttribute>();

                        if (VarCharAnnotion != null)
                        {
                            propEntity.IsUnicode(false);
                        }
                        else
                        {
                            propEntity.IsUnicode(true);

                        }
                        continue;
                    }
                    if (clrType == typeof(decimal) || clrType == typeof(decimal?))
                    {
                        var Precision = prop.FindAnnotation<PrecisionAttribute>();

                        if (Precision == null)
                        {
                            propEntity.HasPrecision(18, 6);
                        }
                        continue;
                    }
                    var JsonDataAttribute = prop.FindAnnotation<JsonDataAttribute>();
                    if (JsonDataAttribute != null)
                    {
                        propEntity.HasColumnType("nvarchar(max)").HasJsonPropertyName("JsData" + prop.Name);
                        continue;
                    }
                    if (clrType.IsClass)
                    {
                        propEntity.HasColumnType("nvarchar(max)").HasJsonPropertyName("JsData" + prop.Name);
                    }
                }

            }
            base.OnModelCreating(modelBuilder);
        }
        public void CommitTransaction()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.CurrentTransaction.Commit();
            }
        }
    }
}
