using VideoTube.Models;

namespace VideoTube.Utils.Builders;

public class CategoryBuilder : BaseBuilder<Category>
{
    public override Category Build()
    {
        var category = new Category
        {
            Name = "Games"
        };

        return category;
    }
}
