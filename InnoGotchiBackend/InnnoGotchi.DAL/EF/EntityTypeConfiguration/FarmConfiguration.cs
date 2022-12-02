using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnnoGotchi.DAL.EF.EntityTypeConfiguration
{
    public class FarmConfiguration : IEntityTypeConfiguration<Farm>
    {
        public void Configure(EntityTypeBuilder<Farm> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id);
            builder.Property(f => f.Name);
            builder.Property(f => f.CreateTime);
            builder.HasOne(f => f.Owner)
                   .WithOne(u => u.Farm);
            builder.HasMany(f => f.Pets)
                   .WithOne(p => p.Farm)
                   .HasForeignKey(p => p.FarmId);
            builder.HasMany(f => f.Сollaborators)
                   .WithMany(u => u.CollaboratedFarms);
        }
    }
}
