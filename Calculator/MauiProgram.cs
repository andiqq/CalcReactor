using Calculator.Components;
#if MACCATALYST
using CoreGraphics;
using UIKit;
#endif
using AppTheme = Calculator.Resources.Styles.AppTheme;
using IWindow = Microsoft.Maui.IWindow;

namespace Calculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<HomePage>(app =>
                {
                    app.UseTheme<AppTheme>();
                })
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
            });
#endif
#if WINDOWS
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, _) =>
            {
                var nativeWindow = handler.PlatformView;
                var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = AppWindow.GetFromWindowId(windowId);

                var size = new SizeInt32(400, 800);
                appWindow.Resize(size);

                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsResizable = false;
                    presenter.SetMinSize(size);
                    presenter.SetMaxSize(size);
                }
            });
#endif
            return builder.Build();
        }
    }
}