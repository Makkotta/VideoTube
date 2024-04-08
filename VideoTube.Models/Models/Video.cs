namespace VideoTube.Models;

public class Video : Entity
{ 
    public string Thumbnail { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;


    public int ChannelId { get; set; }
    public Channel Channel { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
