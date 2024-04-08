using Microsoft.EntityFrameworkCore;

using VideoTube.Models;

namespace VideoTube.Persistence.Repository
{
    public class VideoRepository : IRepository<Video>
    {
        private readonly VideoContext context;

        public VideoRepository(VideoContext context)
        {
            this.context = context;
        }

        public async Task<Video> GetById(int id)
        {
            return await context.Videos.FirstOrDefaultAsync(video => video.Id == id);
        }
    }
}
