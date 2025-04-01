using Microsoft.Maui.Controls;
namespace Calculator.Resources.Styles;

internal class AppTheme : Theme
{
    public static void ToggleCurrentAppTheme()
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = IsDarkTheme ? Microsoft.Maui.ApplicationModel.AppTheme.Light : Microsoft.Maui.ApplicationModel.AppTheme.Dark;
        }
    }

    public static Color DarkBackground { get; } = Color.FromArgb("#FF17171C");
    public static Color DarkText => Colors.White;
    public static Color DarkButtonHigh { get; } = Color.FromArgb("#FF4B5EFC");
    public static Color DarkButtonMedium { get; } = Color.FromArgb("#FF4E505F");
    public static Color DarkButtonLow { get; } = Color.FromArgb("#FF2E2F38");
    public static Color LightBackground { get; } = Color.FromArgb("#FFF1F2F3");
    public static Color LightText => Colors.Black;
    public static Color LightButtonHigh { get; } = Color.FromArgb("#FF4B5EFC");
    public static Color LightButtonMedium { get; } = Color.FromArgb("#FFD2D3DA");
    public static Color LightButtonLow => Colors.White;
    public static Color GeneralWhite => Colors.White;


    public static Color Background => IsDarkTheme ? DarkBackground : LightBackground;
    public static Color Text => IsDarkTheme ? DarkText : LightText;
    public static Color ButtonHighBackground => IsDarkTheme ? DarkButtonHigh : LightButtonHigh;
    public static Color ButtonMediumBackground => IsDarkTheme ? DarkButtonMedium : LightButtonMedium;
    public static Color ButtonLowBackground => IsDarkTheme ? DarkButtonLow : LightButtonLow;

    public static class Selector
    {
        public const string High = nameof(High);
        public const string Medium = nameof(Medium);
        public const string Low = nameof(Low);
    }

    protected override void OnApply()
    {
        LabelStyles.Default = _ => _
            .FontFamily("WorkSansLight")
            .TextColor(Text);

        ButtonStyles.Default = _ => _
            .FontFamily("WorkSansRegular")
            .TextColor(Text)
            .CornerRadius(24)
            .FontSize(32);

        ButtonStyles.Themes[High] = _ => _
            .TextColor(GeneralWhite)
            .BackgroundColor(ButtonHighBackground);

        ButtonStyles.Themes[Medium] = _ => _
            .BackgroundColor(ButtonMediumBackground);

        ButtonStyles.Themes[Low] = _ => _
            .BackgroundColor(ButtonLowBackground);
    }
}
