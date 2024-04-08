using VideoTube.Models;

namespace VideoTube.Persistence.Repository;

public interface IRepository<T> where T : Entity
{
    Task<T> GetById(int id);
}
