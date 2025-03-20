using AppTheme = Calculator.Resources.Styles.AppTheme;
using Button = MauiReactor.Button;
using ImageButton = MauiReactor.ImageButton;

namespace Calculator.Components;

internal partial class KeyPad : Component
{
    [Prop] private Action<string>? _onKeyPressed;

    public override VisualNode Render()
        => Grid("* * * * *", " * * * *",
                RenderButton("C", 2, 0, 0),
                RenderImageButton(Theme.IsDarkTheme ? "plus_minus_white.png" : "plus_minus.png", "+-", 0, 1),
                RenderButton("%", 2, 0, 2),
                RenderButton("÷", 3, 0, 3),
                RenderButton("7", 1, 1, 0),
                RenderButton("8", 1, 1, 1),
                RenderButton("9", 1, 1, 2),
                RenderButton("×", 3, 1, 3),
                RenderButton("4", 1, 2, 0),
                RenderButton("5", 1, 2, 1),
                RenderButton("6", 1, 2, 2),
                RenderButton("-", 3, 2, 3),
                RenderButton("1", 1, 3, 0),
                RenderButton("2", 1, 3, 1),
                RenderButton("3", 1, 3, 2),
                RenderButton("+", 3, 3, 3),
                RenderButton(".", 1, 4, 0),
                RenderButton("0", 1, 4, 1),
                RenderButton("⌫", 1, 4, 2),
                RenderButton("=", 3, 4, 3)
            )
            .ColumnSpacing(16)
            .RowSpacing(16)
            .Margin(20, 0, 20, 20)
            .OnSizeChanged(Invalidate)
            .HeightRequest(400);

    private Button RenderButton(string text, int type, int row, int column)
        => Button(text)
            .ThemeKey(type switch
            {
                1 => AppTheme.Selector.LowEmphasis,
                2 => AppTheme.Selector.MediumEmphasis,
                3 => AppTheme.Selector.HighEmphasis,
                _ => AppTheme.Selector.LowEmphasis
            })
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));

    private ImageButton RenderImageButton(string imageSource, string text, int row, int column)
        => ImageButton()
            .Source(imageSource)
            .Aspect(Aspect.Center)
            .BackgroundColor(AppTheme.ButtonMediumEmphasisBackground)
            .CornerRadius(24)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));
}