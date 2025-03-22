using Calculator.Components;
using Plugin.Maui.KeyListener;
#if MACCATALYST
using CoreGraphics;
using UIKit;
#endif
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
#endif
using IWindow = Microsoft.Maui.IWindow;
using AppTheme = Calculator.Resources.Styles.AppTheme;


namespace Calculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<HomePage>(app => { app.UseTheme<AppTheme>(); })
#if MACCATALYST || WINDOWS
                .UseKeyListener()
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("WorkSans-Regular", "WorkSansRegular");
                    fonts.AddFont("WorkSans-Light.ttf", "WorkSansLight");
                });
#if MACCATALYST
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, _) =>
            {
                if (handler.PlatformView is not { } window) return;
                var windowSize = new CGSize(400, 800);

                window.WindowScene!.SizeRestrictions!.MinimumSize = windowSize;
                window.WindowScene.SizeRestrictions!.MaximumSize = windowSize;

                var screen = UIScreen.MainScreen.Bounds;
                var x = (screen.Width - windowSize.Width) / 2;
                var y = (screen.Height - windowSize.Height) / 2;
                window.Frame = new CGRect(x, y, windowSize.Width, windowSize.Height);
                Console.WriteLine("AppListening");
            });
#endif
#if WINDOWS
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("CustomSwitch", (handler, view) =>
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
                    if (handler.PlatformView is Microsoft.UI.Xaml.Controls.ToggleSwitch toggleSwitch)
                    {
                        toggleSwitch.MinWidth = 0;
                        toggleSwitch.Width = 40;
                    }
                }
            });

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, _) =>
            {
                var nativeWindow = handler.PlatformView as Microsoft.UI.Xaml.Window;
                if (nativeWindow == null) return;

                nativeWindow.Activate();

                var windowHandle = WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = AppWindow.GetFromWindowId(windowId);

                // Set window size to 800x1600, scale factor 2
                var size = new SizeInt32(800, 1600);
                appWindow.Resize(size);

                // Center the window
                var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
                if (displayArea is not null)
                {
                    var centerX = (displayArea.WorkArea.Width - size.Width) / 2;
                    var centerY = (displayArea.WorkArea.Height - size.Height) / 2;
                    appWindow.Move(new PointInt32(centerX, centerY));
                }

                // Prevent window resizing
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter is not null)
                {
                    presenter.IsResizable = false;
                    presenter.IsMaximizable = false;
                }
            });
#endif
            return builder.Build();
        }
    }
}