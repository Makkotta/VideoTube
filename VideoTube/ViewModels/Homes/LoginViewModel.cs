﻿namespace VideoTube.ViewModels.Homes;

using System.ComponentModel.DataAnnotations;
public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
