namespace VideoTube.ViewModels.Channels;

using VideoTube.Models;

public class ChannelViewModel
{
    public Channel Channel { get; set; }
    public List<Video> Videos { get; set; } = new List<Video>();
}
