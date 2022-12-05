using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnnoGotchi.DAL.EF.EntityTypeConfiguration
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name);
            builder.Property(p => p.CreateTime);
            builder.OwnsOne(p => p.Appearance);
            builder.Property(p => p.DeadTime);
            builder.Property(p => p.FeedingCount);
            builder.Property(p => p.DrinkingCount);
            builder.Property(p => p.FirstHappinessDate);
            builder.Property(p => p.LastFeedingTime);
            builder.Property(p => p.LastDrinkingTime);
            builder.Property(p => p.FarmId);

            builder.HasOne(p => p.Farm)
                   .WithMany(f => f.Pets)
                   .HasForeignKey(p => p.FarmId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
