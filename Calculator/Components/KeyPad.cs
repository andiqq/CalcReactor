using Button = MauiReactor.Button;
using ImageButton = MauiReactor.ImageButton;

namespace Calculator.Components;

internal partial class KeyPad : Component
{
    [Prop] private Action<string>? _onKeyPressed;

    public override VisualNode Render()
        => Grid("* * * * *", " * * * *",
                RenderButton("C", Medium, 0, 0),
                RenderImageButton(IsDarkTheme ? "plus_minus_white.png" : "plus_minus.png", "+-", 0, 1),
                RenderButton("%", Medium, 0, 2),
                RenderButton("÷", High, 0, 3),
                RenderButton("7", Low, 1, 0),
                RenderButton("8", Low, 1, 1),
                RenderButton("9", Low, 1, 2),
                RenderButton("×", High, 1, 3),
                RenderButton("4", Low, 2, 0),
                RenderButton("5", Low, 2, 1),
                RenderButton("6", Low, 2, 2),
                RenderButton("-", High, 2, 3),
                RenderButton("1", Low, 3, 0),
                RenderButton("2", Low, 3, 1),
                RenderButton("3", Low, 3, 2),
                RenderButton("+", High, 3, 3),
                RenderButton(".", Low, 4, 0),
                RenderButton("0", Low, 4, 1),
                RenderButton("⌫", Low, 4, 2),
                RenderButton("=", High, 4, 3)
            )
            .ColumnSpacing(16)
            .RowSpacing(16)
            .Margin(20, 0, 20, 20)
            .OnSizeChanged(Invalidate)
            .HeightRequest(400);

    private Button RenderButton(string text, string theme, int row, int column)
        => Button(text)
            .ThemeKey(theme)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));

    private ImageButton RenderImageButton(string imageSource, string text, int row, int column)
        => ImageButton()
            .Source(imageSource)
            .Aspect(Aspect.Center)
            .BackgroundColor(ButtonMediumBackground)
            .CornerRadius(24)
            .GridRow(row)
            .GridColumn(column)
            .OnClicked(() => _onKeyPressed?.Invoke(text));
}