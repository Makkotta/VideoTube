using Bogus;

namespace VideoTube.Utils.Builders;

public abstract class BaseBuilder<T>
{
    protected Faker facker = new Faker();
    public abstract T Build();
}
