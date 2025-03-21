namespace Calculator.Components;

internal class ThemeToggle : Component
{
    public override VisualNode Render()
        => HStack(
            Image("sun.png")
                .HeightRequest(20)
                .WidthRequest(20)
                .GridColumn(0),
            Switch()
                .OnColor(Colors.Gray)
                .IsToggled(IsDarkTheme)
                .OnToggled(ToggleCurrentAppTheme)
                .ThumbColor(ButtonMediumBackground)
                .Margin(10),
            Image("moon.png")
                .HeightRequest(20)
                .WidthRequest(20)
                .GridColumn(2)
        )
            .HCenter()
            .HCenter();
}