using VideoTube.Models;

namespace VideoTube.ViewModels.Homes;

public class HomeViewModel
{
    public List<HomeChannelViewModel> HomeChannels { get; set; } = new List<HomeChannelViewModel>();
    public List<Video> LastVideos { get; set; } = new List<Video>();
}
