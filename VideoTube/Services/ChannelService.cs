using Microsoft.EntityFrameworkCore;

using VideoTube.Models;
using VideoTube.Persistence;
using VideoTube.ViewModels.Homes;

namespace VideoTube.Services;

public class ChannelService
{
    private readonly VideoContext videoContext;

    public ChannelService(VideoContext videoContext)
    {
        this.videoContext = videoContext;
    }

    public async Task<List<Video>> GetAllVideosAsync()
    {
        return await videoContext.Videos
            .Include(video => video.Categories)
            .OrderByDescending(video => video.Created).Take(12).ToListAsync();
    }

    public async Task<Video> GetVideoByIdAsync(int id)
    {
        return await videoContext.Videos
            .Include(video => video.Categories)
            .Include(video => video.Channel)
            .FirstOrDefaultAsync(video => video.Id == id);
    }

    public async Task<Video> UploadVideoAsync(string title, string description, string source, string? thumbnail, int channelId, int[] categories) 
    {
        var video = new Video
        {
            Thumbnail = thumbnail,
            Title = title,
            Description = description,
            Source = source,
            ChannelId = channelId,
        };

        await videoContext.Videos.AddAsync(video);

        foreach (var category in categories.Select(categoryId => new Category { Id = categoryId })) 
        {
            videoContext.Categories.Attach(category);

            video.Categories.Add(category);
        }

        await videoContext.SaveChangesAsync();

        return video;
    }

    public async Task<List<Video>> GetTopVideoByChannelIdAsync(int channelId)
    {
        return await videoContext.Videos
            .Include(video => video.Channel)
            .Include(video => video.Categories)
            .Where(video => video.ChannelId == channelId)
            .OrderByDescending(video => video.Created)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Channel> GetChannelByNameAsync(string channelName) 
    {
        return await videoContext.Channels.FirstOrDefaultAsync(channel => channel.Name == channelName);
    } 

    public async Task<List<Channel>> GetAllChannelsAsync()
    {
        return await videoContext.Channels.Take(10).ToListAsync();
    }

    public async Task<Channel> GetChannelByIdAsync(int id)
    {
        return await videoContext.Channels
            .Include(channel => channel.Videos.Take(10))
            .FirstOrDefaultAsync(channel => channel.Id == id);
    }

    public async Task<Channel> CreateChannelAsync(Channel channel) 
    {
        await videoContext.Channels.AddAsync(channel);

        await videoContext.SaveChangesAsync();

        return channel;
    }


    public async Task<HomeViewModel> GetChannelsAndVideosForHomePageAsync()
    {
        var channels = await videoContext.Channels
            .Take(10)
            .Select(channel => new HomeChannelViewModel
            {
                Channel = channel,
                VideoCount = channel.Videos.Count()
            }).ToListAsync();

        var videos = await videoContext.Videos
            .Include(video => video.Channel)
            .Take(10)
            .OrderByDescending(video => video.Created)
            .ToListAsync();

        return new HomeViewModel { HomeChannels = channels, LastVideos = videos };
    }

    public async Task<List<Category>> GetAllCategoriesAsync() 
    {
        return await videoContext.Categories.OrderBy(category => category.Name).ToListAsync();
    }
}
