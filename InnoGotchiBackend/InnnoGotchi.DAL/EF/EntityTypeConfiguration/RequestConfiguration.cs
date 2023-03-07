using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnnoGotchi.DAL.EF.EntityTypeConfiguration
{
    public class RequestConfiguration : IEntityTypeConfiguration<ColoborationRequest>
    {
        public void Configure(EntityTypeBuilder<ColoborationRequest> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.IsConfirmed);
            builder.HasOne(r => r.RequestOwner)
                   .WithMany(o => o.SentRequests);
            builder.HasOne(r => r.RequestReceipient)
                   .WithMany(r => r.ReceivedRequests);
        }
    }
}
