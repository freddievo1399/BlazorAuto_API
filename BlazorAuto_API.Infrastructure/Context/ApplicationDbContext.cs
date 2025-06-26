using System.Threading.Tasks;
using Azure.Core;
using BlazorAuto_API.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Infrastructure
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        IDbContextTransaction? _transaction = null;
        static DbContextOptions<ApplicationDbContext> _option;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _option = options;
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
        public ApplicationDbContext BeginTransaction()
        {
            var transaction = new ApplicationDbContext(_option);
            if (_transaction != null)
            {
                _transaction = transaction.Database.BeginTransaction();
            }
            return transaction;
        }
        public ApplicationDbContext Connection()
        {
            var transaction = new ApplicationDbContext(_option);
            return transaction;
        }

        public async Task<PagedResultsOf<T>> GetData<T>(DataRequestDto DataManagerDto, Action<IQueryable<T>>? queryExtend = null) where T : EntityBase
        {
            using (var transaction = Connection())
            {
                var DataManagerRequest = DataManagerDto.ToRequest();
                var query = transaction.Set<T>().AsQueryable();
                if (queryExtend != null)
                {
                    queryExtend.Invoke(query);
                }


                if (DataManagerRequest.Where != null && DataManagerRequest.Where.Any())
                    query = DataOperations.PerformFiltering(query, DataManagerRequest.Where, "and");

                if (DataManagerRequest.Search != null && DataManagerRequest.Search.Any())
                    query = DataOperations.PerformSearching(query, DataManagerRequest.Search);

                if (DataManagerRequest.Sorted != null && DataManagerRequest.Sorted.Any())
                    query = DataOperations.PerformSorting(query, DataManagerRequest.Sorted);

                var count = await query.AsNoTracking().CountAsync();

                if (DataManagerRequest.Skip > 0)
                    query = query.Skip(DataManagerRequest.Skip);
                if (DataManagerRequest.Take > 0)
                    query = query.Take(DataManagerRequest.Take);

                var result = await query.AsNoTracking().ToListAsync();
                return PagedResultsOf<T>.Ok(result, count);
            }
        }

        public override async ValueTask DisposeAsync()
        {

            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await base.DisposeAsync();
        }

    }
}
