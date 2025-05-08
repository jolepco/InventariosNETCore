using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Shared.Entities;
using System.Reflection.Emit;

namespace Orders.Backend.Data.ConfigEntities
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(s => new { s.Code, s.Name }).IsUnique();
            builder.HasIndex(s => new { s.StateId, s.Name }).IsUnique();
            builder.HasOne<State>(s => s.State)
                        .WithMany(g => g.ListCities)
                        .HasForeignKey(s => s.StateId);
        }
    }
}
