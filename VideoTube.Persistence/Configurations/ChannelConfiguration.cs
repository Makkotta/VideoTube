

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using VideoTube.Models;

namespace VideoTube.Persistence.Configurations;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.Property(channel => channel.Id).ValueGeneratedOnAdd();
        builder.Property(channel => channel.Name).IsRequired();

        builder.HasMany(channel => channel.Videos).WithOne(video => video.Channel).HasForeignKey(video => video.ChannelId);
    }
}
