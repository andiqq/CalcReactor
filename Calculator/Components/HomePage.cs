using System.Diagnostics.CodeAnalysis;
using AppTheme = Calculator.Resources.Styles.AppTheme;
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
            .BackgroundColor(AppTheme.Background);

    private VStack RenderDisplayPanel()
        => VStack(
                Label(State.ExpressionLabel)
                    .FontSize(40)
                    .TextColor(AppTheme.Text.WithAlpha(0.4f))
                    .HorizontalTextAlignment(TextAlignment.End),
                Label(State.ResultLabel)
                    .FontSize(63)
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
            s.ResultLabel = _calculator.CurrentInput;
        });
    }
}