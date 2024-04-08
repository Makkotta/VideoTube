using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using VideoTube.Models;

namespace VideoTube.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.Property(video => video.Title).IsRequired();
        builder.Property(video => video.Source).IsRequired();

        builder.HasMany(video => video.Categories).WithMany(category => category.Videos);
    }
}
