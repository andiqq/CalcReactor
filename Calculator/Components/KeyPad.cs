using AppTheme = Calculator.Resources.Styles.AppTheme;
using Button = MauiReactor.Button;
using Grid = MauiReactor.Grid;
namespace Calculator.Components;

internal partial class KeyPad : Component
{
 [Prop] private Action<string>? _onKeyPressed;

    public override VisualNode Render()
        => Grid(
                RenderButtonMediumEmphasis("C", 0, 0),
                RenderImageButtonMediumEmphasis(Theme.IsDarkTheme ? "plus_minus_white.png" : "plus_minus.png", "+-",
                    0, 1),
                RenderButtonMediumEmphasis("%", 0, 2),
                RenderButtonHighEmphasis("÷", 0, 3),
                RenderButtonLowEmphasis("7", 1, 0),
                RenderButtonLowEmphasis("8", 1, 1),
                RenderButtonLowEmphasis("9", 1, 2),
                RenderButtonHighEmphasis("×", 1, 3),
                RenderButtonLowEmphasis("4", 2, 0),
                RenderButtonLowEmphasis("5", 2, 1),
                RenderButtonLowEmphasis("6", 2, 2),
                RenderButtonHighEmphasis("-", 2, 3),
                RenderButtonLowEmphasis("1", 3, 0),
                RenderButtonLowEmphasis("2", 3, 1),
                RenderButtonLowEmphasis("3", 3, 2),
                RenderButtonHighEmphasis("+", 3, 3),
                RenderButtonLowEmphasis(".", 4, 0),
                RenderButtonLowEmphasis("0", 4, 1),
                RenderImageButtonLowEmphasis(Theme.IsDarkTheme ? "back_white.png" : "back.png", "back", 4, 2),
                RenderButtonHighEmphasis("=", 4, 3)
            )
            .Rows("* * * * *")
            .Columns("* * * *")
            .ColumnSpacing(16)
            .RowSpacing(16)
            .Margin(20, 0, 20, 20)
            .OnSizeChanged(Invalidate)
            .HeightRequest(400);

    private Button RenderButtonLowEmphasis(string text, int row, int column)
        => Button(text)
            .ThemeKey(AppTheme.Selector.LowEmphasis)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));

    private Button RenderButtonMediumEmphasis(string text, int row, int column)
        => Button(text)
            .ThemeKey(AppTheme.Selector.MediumEmphasis)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));

    private Grid RenderImageButtonMediumEmphasis(string imageSource, string text, int row, int column)
        => AppTheme.ImageButtonMediumEmphasis(imageSource, () => _onKeyPressed?.Invoke(text))
            .GridRow(row)
            .GridColumn(column);

    private Grid RenderImageButtonLowEmphasis(string imageSource, string text, int row, int column)
        => AppTheme.ImageButtonLowEmphasis(imageSource, () => _onKeyPressed?.Invoke(text))
            .GridRow(row)
            .GridColumn(column);

    private Button RenderButtonHighEmphasis(string text, int row, int column)
        => Button(text)
            .ThemeKey(AppTheme.Selector.HighEmphasis)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));
}