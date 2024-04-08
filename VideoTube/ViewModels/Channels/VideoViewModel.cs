namespace VideoTube.ViewModels.Channels;

using VideoTube.Models;

public class VideoViewModel
{
    public Video Video { get; set; }
    public List<Video> Others { get; set; } = new List<Video>();
}
