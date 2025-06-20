
using Microsoft.EntityFrameworkCore;

namespace BlazorAuto_API.AbstractServer;
public class EntityRegister : IEntityRegister
{
    public void RegisterEntities(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<EntityProductCategory>()
           .HasIndex(c => new { c.ProductId, c.CategoryId }).IsUnique();

        modelbuilder.Entity<EntityCategory>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryId)
            .HasPrincipalKey(x=>x.Id);

        modelbuilder.Entity<EntityProduct>()
            .HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .HasPrincipalKey(x=>x.Id);




        modelbuilder.Entity<ProductSpecification>()
            .HasOne(c => c.Product)
            .WithMany(pc => pc.Specifications)
            .HasForeignKey(x=>x.ProductId);

    }
}

