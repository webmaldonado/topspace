using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Maui.LifecycleEvents;
using TopSpaceMAUI.ViewModel;
using TopSpaceMAUI.Views;

namespace TopSpaceMAUI
{
    public static class MauiProgram
    {
        public static IPublicClientApplication PCA;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkitCamera().UseMauiCommunityToolkitMediaElement();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            DAL.Database.CreateDatabase();

            PCA = PublicClientApplicationBuilder.Create("0031ebbc-d210-4326-a3f7-a6bae8cb6991")
                    .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                    .WithRedirectUri("mauiapp://auth")
                    .Build();

            return builder.Build();
        }
    }
}