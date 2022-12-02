using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnnoGotchi.DAL.EF.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id);
            builder.Property(u => u.FirstName);
            builder.Property(u => u.LastName);
            builder.Property(u => u.AvatarURL);
            builder.Property(u => u.Email);
            builder.HasMany(u => u.ColoborationRequests)
                   .WithOne(r => r.RequestOwner)
                   .HasForeignKey(r => r.RequestOwnerId);
            builder.HasMany(u => u.CollaboratedFarms)
                   .WithMany(f => f.Сollaborators);
            builder.HasOne(u => u.Farm)
                   .WithOne(f => f.Owner);
        }
    }
}
