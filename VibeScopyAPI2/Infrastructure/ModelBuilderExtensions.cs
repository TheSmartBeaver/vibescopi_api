using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

public static class ModelBuilderExtensions
{
    public static void CreateEnumMapping<TEntity, TEnum>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntity, ICollection<TEnum>>> propertyExpression) where TEntity : class
    {
        var entityBuilder = modelBuilder.Entity<TEntity>();

        var property = entityBuilder.Property(propertyExpression);

        var valueComparer = new ValueComparer<ICollection<TEnum>>(
            (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
            c => c == null ? 0 : JsonConvert.SerializeObject(c).GetHashCode(),
            c => JsonConvert.DeserializeObject<ICollection<TEnum>>(JsonConvert.SerializeObject(c))
        );

        property.HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(s => (TEnum)Enum.Parse(typeof(TEnum), s))
                  .ToList()
        ).Metadata.SetValueComparer(valueComparer);

        //TODO: Ajouter un value comparer ici pour éviter warning dans génération migration, hors c'est un non-sens de définir un ordre dans des enums, ajouter comparer fake ??
    }
}