using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeScopyAPI.Models;

public static class ModelBuilderExtensions
{
    public static void CreateEnumMapping<TEntity, TEnum>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntity, ICollection<TEnum>>> propertyExpression) where TEntity : class
    {
        var entityBuilder = modelBuilder.Entity<TEntity>();

        var property = entityBuilder.Property(propertyExpression);

        property.HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(s => (TEnum)Enum.Parse(typeof(TEnum), s))
                  .ToList()
        );
    }
}