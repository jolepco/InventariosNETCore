using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Shared.Entities;

namespace Orders.Backend.Data.ConfigEntities
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           // builder.HasKey(x => x.Id);
            builder.HasOne<City>(s => s.City)
                        .WithMany(g => g.Users)
                        .HasForeignKey(s => s.CityId);
        }
    }
}
