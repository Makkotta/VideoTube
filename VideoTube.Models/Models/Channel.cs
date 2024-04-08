namespace VideoTube.Models;

public class Channel : Entity
{
    public string Name { get; set; }
    public string Logo { get; set; }

    public ICollection<Video> Videos { get; set; } = new List<Video>();
}
