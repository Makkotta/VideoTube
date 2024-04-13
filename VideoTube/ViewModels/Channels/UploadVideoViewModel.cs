using System.ComponentModel.DataAnnotations;

using VideoTube.Models;

namespace VideoTube.ViewModels.Channels;

public class UploadVideoViewModel
{
    [Required]
    [Display(Name = "Video File")]
    public IFormFile VideoFile { get; set; }

    [Display(Name = "Thumbnail")]
    public IFormFile Thumbnail { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 3)]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required]
    [StringLength(500)]
    [Display(Name = "Description")]
    public string Description { get; set; }

    public int[] Category { get; set; }

    public IReadOnlyCollection<Category> Categories { get; set; } = new List<Category>();
}
