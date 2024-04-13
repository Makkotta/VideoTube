using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using VideoTube.Models;
using VideoTube.Services;
using VideoTube.ViewModels.Homes;

namespace VideoTube.Controllers;

public class HomeController : Controller
{
    private readonly ChannelService channelService;

    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IUserStore<IdentityUser> userStore;

    public HomeController(
        ChannelService channelService,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore)
    {
        this.channelService = channelService;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userStore = userStore;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetHomePage()
    {
        var viewModel = await channelService.GetChannelsAndVideosForHomePageAsync();

        var user = await userManager.GetUserAsync(HttpContext.User);

        return View("Home", viewModel);
    }

    [HttpGet("/login")]
    public async Task<IActionResult> GetLoginPageAsync()
    {
        await Task.FromResult(0);

        return View("Login");
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LoginAsync(LoginViewModel input)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(input.UserName, input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect("/");
            }
        }

        return LocalRedirect("/login");
    }

    [HttpGet("/logout")]
    public async Task<IActionResult> LogoutAsync() 
    {
        await signInManager.SignOutAsync();

        return LocalRedirect("/");
    }

    [HttpGet("/register")]
    public IActionResult GetRegisterPage()
    {
        return View("Register");
    }

    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel input)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser();

            await userStore.SetUserNameAsync(user, input.LoginName, CancellationToken.None);
            var result = await userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                var channel = new Channel { Name = user.UserName };
                await channelService.CreateChannelAsync(channel);

                return LocalRedirect("/");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("Register");
    }
}
