using Calculator.Components;
using Plugin.Maui.KeyListener;
#if MACCATALYST
using CoreGraphics;
#endif
#if WINDOWS
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
using IWindow = Microsoft.Maui.IWindow;
using AppTheme = Calculator.Resources.Styles.AppTheme;
using System.Globalization;


namespace Calculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<HomePage>(app => { app.UseTheme<AppTheme>(); })

#if MACCATALYST || WINDOWS
                // Registering the KeyListener plugin for Mac Catalyst and WinUI for handling keyboard events
                .UseKeyListener()
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("WorkSans-Regular", "WorkSansRegular");
                    fonts.AddFont("WorkSans-Light.ttf", "WorkSansLight");
                });
#if MACCATALYST
            // Customizing the Window for Mac Catalyst
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, _) =>
            {
                if (handler.PlatformView is not { } window) return;
                var windowSize = new CGSize(400, 800);

                window.WindowScene!.SizeRestrictions!.MinimumSize = windowSize;
                window.WindowScene.SizeRestrictions!.MaximumSize = windowSize;
            });
#endif
#if WINDOWS
            // Customizing the Window for WinUI
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, _) =>
            {
                if (handler.PlatformView.AppWindow is not { } appWindow) return;

                var windowSize = new SizeInt32(800, 1600);

                appWindow.Resize(windowSize);

                if (appWindow.Presenter is not OverlappedPresenter presenter) return;

                presenter.IsResizable = false;
                presenter.IsMaximizable = false;

            });

            // Customizing the Switch control for WinUI
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("CustomSwitch", (handler, view) =>
            {
                if (DeviceInfo.Current.Platform != DevicePlatform.WinUI) return;

                if (handler.PlatformView is not Microsoft.UI.Xaml.Controls.ToggleSwitch toggleSwitch) return;
                    
                toggleSwitch.MinWidth = 0;
                toggleSwitch.Width = 40;
             });
#endif
            return builder.Build();
        }
    }
}