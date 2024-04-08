using VideoTube.Models;

namespace VideoTube.Utils.Builders;

public class VideoBuilder : BaseBuilder<Video>
{
    private BaseBuilder<Category> categoryBuilder = new CategoryBuilder();
    public override Video Build()
    {
        var video = new Video
        {
            Id = facker.Random.Int(),
            Title = facker.Random.Word(),
            Description = facker.Random.Words(),

            Created = facker.Date.Past(),

            Categories = Enumerable.Range(0, 3).Select(_ => categoryBuilder.Build()).ToList()
        };

        return video;
    }
}
