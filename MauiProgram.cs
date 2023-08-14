// Fix the minimum and maximum window sizes
// Ref: https://github.com/dotnet/maui/discussions/2370#discussioncomment-6701453

using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using WinUIEx;

#elif MACCATALYST
using CoreGraphics;
using UIKit;

#elif IOS
#elif ANDROID
#elif TIZEN

#endif

namespace MauiPlayground;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Needs using Microsoft.Maui.LifecycleEvents for this extension method
        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            // using WinUIEx
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    window.CenterOnScreen(1024,768); //uses WinUIEx
                });
            });

#elif MACCATALYST            
            // Using CoreGraphics and UIKit
            events.AddiOS(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.SceneWillConnect((scene, session, options) =>
                {
                    if (scene is UIWindowScene { SizeRestrictions: { } } windowScene)
                    {
                        windowScene.SizeRestrictions.MaximumSize = new CGSize(1200, 900);
                        windowScene.SizeRestrictions.MinimumSize = new CGSize(600, 400);
                    }
                });

            });
#endif
        });

        return builder.Build();
	}
}

