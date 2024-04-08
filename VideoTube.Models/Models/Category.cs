namespace VideoTube.Models;

public class Category : Entity
{
    public string Name { get; set; }

    public ICollection<Video> Videos { get; set; } = new List<Video>();
}
