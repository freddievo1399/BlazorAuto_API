using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

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
            EntityRegister entityRegisterBase = new();
            entityRegisterBase.RegisterEntities(modelBuilder);
            foreach (var i in entityRegisters)
            {
                i.RegisterEntities(modelBuilder);
            }
            foreach (var modelType in modelBuilder.Model.GetEntityTypes())
            {
                var nameTable = modelType.FindAnnotation("Relational:TableName");
                if (nameTable != null)
                {
                    var entity = modelBuilder.Entity(modelType.Name);
                    entity.ToTable(nameTable.Value?.ToString());
                    entity.HasIndex("Guid").IsUnique();

                    var props = entity.Metadata.GetDeclaredProperties();
                    foreach (IMutableProperty prop in props)
                    {
                        var MaxLength = prop.FindAnnotation<PrecisionAttribute>();
                        if (MaxLength != null)
                        {
                            entity.Property(prop.Name).HasMaxLength(100);
                        }

                        if (prop.ClrType == typeof(decimal) || prop.ClrType == typeof(decimal?))
                        {
                            var Precision = prop.FindAnnotation<PrecisionAttribute>();

                            if (Precision == null)
                            {
                                entity.Property(prop.Name).HasPrecision(18, 6);
                            }
                        }
                        var VarCharAnnotion = prop.FindAnnotation<NvarcharAttribute>();

                        if (VarCharAnnotion != null)
                        {
                            entity.Property(prop.Name).IsUnicode(true);
                        }
                        else
                        {
                            entity.Property(prop.Name).IsUnicode(false);

                        }
                        var JsonDataAttribute = prop.FindAnnotation<JsonDataAttribute>();

                        if (JsonDataAttribute != null)
                        {
                            entity.Property(prop.Name).HasColumnType("nvarchar(max)").HasJsonPropertyName("JsData" + prop.Name);
                        }
                    }

                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
