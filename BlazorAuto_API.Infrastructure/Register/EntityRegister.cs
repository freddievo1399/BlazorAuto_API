
using System.Reflection.Emit;
using BlazorAuto_API.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuto_API.Infrastructure;
public class EntityRegister : IEntityRegister
{
    public void RegisterEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityProductCategory>()
           .HasIndex(c => new { c.ProductGuid, c.CategoryGuid }).IsUnique();

        modelBuilder.Entity<EntityCategory>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryGuid)
            .HasPrincipalKey(x => x.Guid);

        modelBuilder.Entity<EntityProduct>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductGuid)
            .HasPrincipalKey(x => x.Guid);




        modelBuilder.Entity<EntityProductSpecification>()
            .HasOne(c => c.Product)
            .WithMany(pc => pc.Specifications)
            .HasForeignKey(x => x.ProductGuid)
            .HasPrincipalKey(x => x.Guid);

        modelBuilder.Ignore<ImageInfo>();
        modelBuilder.Entity<EntityProduct>()
        .Property(p => p.Images)
        .HasColumnName("ImagesJson")
        .HasColumnType("nvarchar(max)")
        .HasConversion(
                v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
                v => Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImageInfo>>(v) ?? new()
        ).IsUnicode(true)
        ;




        //modelBuilder.Entity<EntityProduct>()
        //    .Property(e => e.Images)
        //    .HasJsonPropertyName("ImageJson").HasColumnType("nvarchar(max)"); ;
        //    .HasColumnName("JsonImages")
        //    .HasConversion(
        //        v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
        //        v => Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImageInfo>>(v) ?? new()
        //);



    }
}

