using System.Diagnostics.CodeAnalysis;
using Calculator.Services;
using Plugin.Maui.KeyListener;
using System.Globalization;

namespace Calculator.Components;


internal class HomePageState
{
    public string ExpressionLabel { get; set; } = string.Empty;
    public string ResultLabel { get; set; } = "0";
}

#if MACCATALYST || WINDOWS
[Scaffold(typeof(Plugin.Maui.KeyListener.KeyboardBehavior))]
partial class KeyboardBehavior
{
}
#endif

[SuppressMessage("ReSharper", "SpecifyACultureInStringConversionExplicitly")]
internal class HomePage : Component<HomePageState>
{
    private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");
    
    private readonly CalculatorEngine _calculator = new();

    public override VisualNode Render()
        => ContentPage(
                Grid("56 * 420", "*",
#if MACCATALYST || WINDOWS
                    new KeyboardBehavior()
                        .OnKeyDown(KeyDown),
#endif
                    new ThemeToggle(),
                    RenderDisplayPanel(),
                    new KeyPad()
                        .OnKeyPressed(OnKeyPressed)
                        .GridRow(2)
                )
            )
            .BackgroundColor(Background);


    private VStack RenderDisplayPanel()
        => VStack(
                Label(State.ExpressionLabel)
                    .FontSize(40)
                    .TextColor(Text.WithAlpha(0.4f))
                    .HorizontalTextAlignment(TextAlignment.End),
                Label(State.ResultLabel)
                    .FontSize(CalculateFontSize(State.ResultLabel).fontSize)
                    .HorizontalTextAlignment(TextAlignment.End)
                    .LineBreakMode(LineBreakMode.NoWrap)
            )
            .Margin(20, 0)
            .GridRow(1)
            .HFill()
            .VEnd();
    
    private void OnKeyPressed(string key)
    {
        _calculator.HandleInput(key);

        SetState(s =>
        {
            s.ExpressionLabel = _calculator.GetExpression();
            s.ResultLabel = key == "." ? _calculator.CurrentInput : FormatNumber(_calculator.CurrentValue);
        });
    }

    private void KeyDown(object? sender, KeyPressedEventArgs e)
    {
        var keyString = e.Keys;
        Console.WriteLine(e.Keys.ToString());
        var key = keyString switch
        {
            KeyboardKeys.Number0 or KeyboardKeys.NumPad0 => "0",
            KeyboardKeys.Number1 or KeyboardKeys.NumPad1 => "1",
            KeyboardKeys.Number2 or KeyboardKeys.NumPad2 => "2",
            KeyboardKeys.Number3 or KeyboardKeys.NumPad3 => "3",
            KeyboardKeys.Number4 or KeyboardKeys.NumPad4 => "4",
            KeyboardKeys.Number5 or KeyboardKeys.NumPad5 => "5",
            KeyboardKeys.Number6 or KeyboardKeys.NumPad6 => "6",
            KeyboardKeys.Number7 or KeyboardKeys.NumPad7 => "7",
            KeyboardKeys.Number8 or KeyboardKeys.NumPad8 => "8",
            KeyboardKeys.Number9 or KeyboardKeys.NumPad9 => "9",
            KeyboardKeys.NumPadPlus or KeyboardKeys.Plus => "+",
            KeyboardKeys.NumPadMinus or KeyboardKeys.Minus => "-",
            KeyboardKeys.NumPadMultiply => "×",
            KeyboardKeys.NumPadDivide => "÷",
            KeyboardKeys.NumPadDecimal or KeyboardKeys.Period => ".",
            KeyboardKeys.Backspace => "⌫",
            KeyboardKeys.Enter or KeyboardKeys.NumPadEnter => "=",
            KeyboardKeys.Escape or KeyboardKeys.C => "C",
            _ => null
        };
        Console.WriteLine(key);

        if (key != null)
        {
            OnKeyPressed(key);
        }
    }
}