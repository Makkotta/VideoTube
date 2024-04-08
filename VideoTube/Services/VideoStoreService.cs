using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace VideoTube.Services;

public class VideoStoreService
{
    private readonly BlobServiceClient blobServiceClient;

    public VideoStoreService(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
    }


    public async Task UploadVideoAsync(string fileName, IFormFile video) 
    {
        var containerClient = this.blobServiceClient.GetBlobContainerClient("videos");

        var file = containerClient.GetBlobClient(fileName);

        await file.UploadAsync(
            video.OpenReadStream(),
            new BlobHttpHeaders { ContentType = "video/mp4" });
    }

    public async Task UploadThumbnailAsync(string fileName, IFormFile thumbnail) 
    {
        var containerClient = this.blobServiceClient.GetBlobContainerClient("thumbnails");

        await containerClient.UploadBlobAsync(fileName, thumbnail.OpenReadStream());
    }
}
