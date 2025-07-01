
using Microsoft.EntityFrameworkCore;

namespace BlazorAuto_API.Infrastructure;
public class EntityRegister : IEntityRegister
{
    public void RegisterEntities(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<EntityProductCategory>()
           .HasIndex(c => new { c.ProductGuid, c.CategoryGuid }).IsUnique();

        modelbuilder.Entity<EntityCategory>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryGuid)
            .HasPrincipalKey(x=>x.Guid);

        modelbuilder.Entity<EntityProduct>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductGuid)
            .HasPrincipalKey(x=>x.Guid);




        modelbuilder.Entity<EntityProductSpecification>()
            .HasOne(c => c.Product)
            .WithMany(pc => pc.Specifications)
            .HasForeignKey(x=>x.ProductGuid)
            .HasPrincipalKey(x=>x.Guid);

    }
}

