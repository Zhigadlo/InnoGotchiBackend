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

            builder.HasMany(u => u.SentRequests)
                   .WithOne(r => r.RequestOwner)
                   .HasForeignKey(r => r.RequestOwnerId)
                   .OnDelete(DeleteBehavior.NoAction); 
            
            builder.HasMany(u => u.ReceivedRequests)
                   .WithOne(r => r.RequestReceipient)
                   .HasForeignKey(r => r.RequestReceipientId)
                   .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(u => u.Farm)
                   .WithOne(f => f.Owner)
                   .HasForeignKey<Farm>(f => f.OwnerId);
        }
    }
}
