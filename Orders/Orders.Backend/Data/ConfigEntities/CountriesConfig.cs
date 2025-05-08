using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Shared.Entities;

namespace Orders.Backend.Data.ConfigEntities
{
    public class CountriesConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => new { c.Name, c.Code }).IsUnique();
            
        }
    }
}
