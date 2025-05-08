using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Shared.Entities;
using System.Reflection.Emit;

namespace Orders.Backend.Data.ConfigEntities
{
    public class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(c => c.Id);
           builder.HasIndex(c => new { c.CountryId, c.Name }).IsUnique();
            builder.HasIndex(c => new { c.Code, c.Name }).IsUnique();
            builder.HasOne<Country>(s => s.Country)
                        .WithMany(g => g.ListStates)
                        .HasForeignKey(s => s.CountryId);
        }
    }
}
