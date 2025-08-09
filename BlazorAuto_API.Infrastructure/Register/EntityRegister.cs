
using System.Reflection.Emit;
using BlazorAuto_API.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorAuto_API.Infrastructure;
public class EntityRegister : IEntityRegister
{
    public void RegisterEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityRefreshTokenEntityBase>().HasIndex(x => x.UserName);
        modelBuilder.Entity<EntityRefreshTokenEntityBase>().HasIndex(x => x.Token);
        modelBuilder.Entity<EntityRefreshTokenEntityBase>().HasKey(x => new { x.UserName, x.Token });


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

        //    .HasJsonPropertyName(, "ImagesJson");
        //modelBuilder.Entity<EntityProduct>().Property(p => p.CarouselImages)
        //    .Hasjso;
        //modelBuilder.Entity<EntityProduct>(entity =>
        //{
        //    entity.OwnsMany(p => p.Images, owned =>
        //    {
        //        owned.ToJson("ImagesJson");
        //    });
        //    entity.OwnsMany(p => p.CarouselImages, owned =>
        //    {
        //        owned.ToJson("CarouselImagesJson");
        //    });
        //});
        //modelBuilder.Entity<EntityProduct>().OwnsMany(p => p.Images, config => { config.ToJson("ImagesJson"); });
        //modelBuilder.Entity<EntityProduct>().OwnsMany(p => p.CarouselImages, config => { config.ToJson("phone_numbers"); });
        var imageInfoComparer = new ValueComparer<List<ImageInfo>>(
    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2), // So sánh nội dung
    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Hash code
    c => c != null ? c.ToList() : new()
);
        modelBuilder.Entity<EntityProduct>()
        .Property(p => p.Images)
        .HasColumnName("ImagesJson")
        .HasColumnType("nvarchar(max)")
        .HasConversion(
                v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
                v => Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImageInfo>>(v) ?? new()
        ).IsUnicode(true).Metadata.SetValueComparer(imageInfoComparer);


        modelBuilder.Entity<EntityProduct>()
  .Property(p => p.CarouselImages)
  .HasColumnName("CarouselImagesJson")
  .HasColumnType("nvarchar(max)")
  .HasConversion(
          v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
          v => Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImageInfo>>(v) ?? new()
  ).IsUnicode(true).Metadata.SetValueComparer(imageInfoComparer);


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

