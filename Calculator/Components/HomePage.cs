using System.Diagnostics.CodeAnalysis;
using Calculator.Services;

namespace Calculator.Components;

internal class HomePageState
{
    public string ExpressionLabel { get; set; } = string.Empty;
    public string ResultLabel { get; set; } = "0";
}

[SuppressMessage("ReSharper", "SpecifyACultureInStringConversionExplicitly")]
internal class HomePage : Component<HomePageState>
{
    private readonly CalculatorEngine _calculator = new();

    public override VisualNode Render()
        => ContentPage(
                Grid("48 * 420", "*",
                    new ThemeToggle(),
                    RenderDisplayPanel(),
                    new KeyPad()
                        .OnKeyPressed(OnKeyPressed)
                        .GridRow(2)
                )
            )
            .BackgroundColor(Background);
    
    private VStack RenderDisplayPanel()
    {
        return VStack(
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
    }

    private void OnKeyPressed(string key)
    {
        _calculator.HandleInput(key);

        SetState(s =>
        {
            s.ExpressionLabel = _calculator.GetExpression();
            s.ResultLabel = key == "." ? _calculator.CurrentInput : FormatNumber(_calculator.CurrentValue);
        });
    }
}