using Microsoft.AspNetCore.Mvc;

using VideoTube.Services;
using VideoTube.ViewModels.Channels;

namespace VideoTube.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ChannelService channelService;
        private readonly VideoStoreService videoStoreService;
        private readonly string videoPath;

        public ChannelController(ChannelService channelService, VideoStoreService videoStoreService)
        {
            this.channelService = channelService;
            this.videoStoreService = videoStoreService;
        }

        [HttpGet("channels")]
        [HttpGet("channels/{id:int}")]
        public async Task<IActionResult> GetChannelAsync(int id)
        {
            if (id == default) 
            {
                var channels = await channelService.GetAllChannelsAsync();
                var channelsViewModel = new ChannelsViewModel { Channels = channels };

                return View("Channels", channelsViewModel);
            }

            var channel = await channelService.GetChannelByIdAsync(id);

            if (channel == null) return NotFound();

            var channelViewModel = new ChannelViewModel
            { 
                Channel = channel,
                Videos = channel.Videos.ToList(),
            };

            return View("Channel", channelViewModel);
        }

        [HttpGet("channels/{channelId:int}/videos/{videoId:int}")]
        public async Task<IActionResult> Video(int channelId, int videoId)
        {
            var video = await channelService.GetVideoByIdAsync(videoId);

            var topVideosOfThisChannel = (await channelService.GetTopVideoByChannelIdAsync(channelId))
                .Where(video => video.Id != videoId).ToList();

            var viewModel = new VideoViewModel { Video = video, Others = topVideosOfThisChannel };

            return View(viewModel);
        }

        [HttpGet("channels/upload")]
        public async Task<IActionResult> GetUploadVideoPageAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true) 
            {
                return LocalRedirect("/login");
            }

            var categories = await channelService.GetAllCategoriesAsync();

            var viewModel = new UploadVideoViewModel { Categories = categories };

            return View("Upload", viewModel);
        }

        [HttpPost("channels/upload")]
        public async Task<IActionResult> UploadVideoAsync(UploadVideoViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View("Upload", model);
            }

            if (model.VideoFile == null || model.VideoFile.Length == 0)
            {
                ModelState.AddModelError("VideoFile", "Please select a video file to upload.");
                return View("Upload", model);
            }

            if (model.VideoFile.Length > 104857600)
            {
                ModelState.AddModelError("VideoFile", "Video size cannot exceed 100 MB.");
                return View("Upload", model);
            }

            var allowedExtensions = new[] { ".mp4", ".webm", ".ogg" };
            if (!allowedExtensions.Contains(Path.GetExtension(model.VideoFile.FileName).ToLower()))
            {
                ModelState.AddModelError("VideoFile", "Invalid video format. Only MP4, WEBM, and OGG are allowed.");
                return View(model);
            }

            var fileId = Guid.NewGuid();
            var videoName = fileId + Path.GetExtension(model.VideoFile.FileName);
            string? thumbnailName = null;

            await videoStoreService.UploadVideoAsync(videoName, model.VideoFile);

            if (model.Thumbnail?.Length > 0)
            {
                thumbnailName = fileId + Path.GetExtension(model.Thumbnail.FileName);
                await videoStoreService.UploadThumbnailAsync(thumbnailName, model.Thumbnail);
            }

            var video = await channelService.UploadVideoAsync(model.Title, model.Description, videoName, thumbnailName, 1);

            return LocalRedirect($"/channels/{video.ChannelId}/videos/{video.Id}");
        }
    }
}
