using Calculator.Components;
using AppTheme = Calculator.Resources.Styles.AppTheme;

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


            return builder.Build();
        }
    }
}