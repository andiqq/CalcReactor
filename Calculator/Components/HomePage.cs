﻿using MauiReactor.Canvas;
using AppTheme = Calculator.Resources.Styles.AppTheme;
using Button = MauiReactor.Button;
using Grid = MauiReactor.Grid;

namespace Calculator.Components;

internal class HomePageState
{
    public string CurrentNumber { get; set; } = string.Empty;

    public double? Number1 { get; set; }

    public double? Number2 { get; set; }

    public string CurrentOperation { get; set; } = string.Empty;

    public double? Result { get; set; }

    public bool Perc { get; set; }
}

internal class HomePage : Component<HomePageState>
{
    public override VisualNode Render()
        => ContentPage(
                Grid("48 * 420", "*",
                    RenderThemeToggle(),
                    RenderDisplayPanel(),
                    new KeyPad()
                        .OnKeyPressed(OnKeyPressed)
                        .GridRow(2)
                )
            )
            .BackgroundColor(AppTheme.Background);

    private VStack RenderDisplayPanel()
        => VStack(
                Label(() =>
                        $"{State.Number1} {State.CurrentOperation} {State.Number2}{(State.Perc ? "%" : string.Empty)}{(State.Result != null ? " =" : string.Empty)}")
                    .FontSize(40)
                    .TextColor(AppTheme.Text.WithAlpha(0.4f))
                    .HorizontalTextAlignment(TextAlignment.End),
                Label(() => State.Result != null ? State.Result.Value.ToString() :
                        State.CurrentNumber.Length > 0 ? State.CurrentNumber : "0")
                    .FontSize(63)
                    .HorizontalTextAlignment(TextAlignment.End)
            )
            .Margin(20, 0)
            .GridRow(1)
            .HFill()
            .VEnd();

    private static CanvasView RenderThemeToggle()
        => new CanvasView
            {
                new Box
                    {
                        new Group
                        {
                            new Align
                                {
                                    new Ellipse()
                                        .FillColor(AppTheme.ButtonMediumEmphasisBackground)
                                }
                                .Height(24)
                                .Width(24)
                                .Margin(4)
                                .HorizontalAlignment(Theme.IsDarkTheme
                                    ? Microsoft.Maui.Primitives.LayoutAlignment.Start
                                    : Microsoft.Maui.Primitives.LayoutAlignment.End)
                                .VCenter(),

                            new Align
                                {
                                    Theme.IsDarkTheme
                                        ? new Picture("Calculator.Resources.Images.moon.png")
                                        : new Picture("Calculator.Resources.Images.sun.png"),
                                }
                                .Height(24)
                                .Width(24)
                                .Margin(8, 4)
                                .HorizontalAlignment(Theme.IsDarkTheme
                                    ? Microsoft.Maui.Primitives.LayoutAlignment.End
                                    : Microsoft.Maui.Primitives.LayoutAlignment.Start)
                                .VCenter()
                        }
                    }
                    .BackgroundColor(AppTheme.ButtonLowEmphasisBackground)
                    .CornerRadius(16)
            }
            .OnTapped(AppTheme.ToggleCurrentAppTheme)
            .Margin(16)
            .VCenter()
            .HCenter()
            .HeightRequest(32)
            .WidthRequest(72)
            .BackgroundColor(Colors.Transparent);

    private void OnKeyPressed(string key)
    {
        if (State.Result != null)
        {
            if (key is "÷" or "×" or "+" or "-")
            {
                SetState(s =>
                {
                    s.Number1 = s.Result;
                    s.Number2 = s.Result = null;
                    s.CurrentOperation = key;
                    s.CurrentNumber = string.Empty;
                    s.Perc = false;
                }, false);
            }
            else
            {
                SetState(s =>
                {
                    s.Number1 = s.Number2 = s.Result = null;
                    s.CurrentOperation = string.Empty;
                    s.CurrentNumber = string.Empty;
                    s.Perc = false;
                }, false);
            }
        }

        ;


        switch (key)
        {
            case "back":
            {
                if (State.CurrentNumber.Length > 0)
                {
                    SetState(s => s.CurrentNumber = s.CurrentNumber.Substring(0, s.CurrentNumber.Length - 1), false);
                }

                break;
            }
            case ".":
            {
                if (State.CurrentNumber.Length > 0 && !State.CurrentNumber.Contains('.'))
                {
                    SetState(s => s.CurrentNumber += key, false);
                }

                break;
            }
            case "0":
            {
                if (State.CurrentNumber.Length > 0)
                {
                    SetState(s => s.CurrentNumber += key, false);
                }

                break;
            }
            case "C":
                SetState(s => s.CurrentNumber = string.Empty);
                break;
            case "=":
            {
                if (State.CurrentOperation.Length > 0 && State.Number1 != null)
                {
                    SetState(s =>
                    {
                        s.Number2 = State.CurrentNumber.Length > 0 ? double.Parse(State.CurrentNumber) : 0.0;
                        switch (s.CurrentOperation)
                        {
                            case "÷":
                                s.Result = s.Number1!.Value / s.Number2.Value;
                                break;
                            case "×":
                                s.Result = s.Number1!.Value * s.Number2.Value;
                                break;
                            case "+":
                                s.Result = s.Number1!.Value + s.Number2.Value;
                                break;
                            case "-":
                                s.Result = s.Number1!.Value - s.Number2.Value;
                                break;
                        }
                    }, false);
                }

                break;
            }
            case "%":
            {
                if (State.CurrentOperation.Length > 0 && State.Number1 != null)
                {
                    SetState(s =>
                    {
                        s.Number2 = State.CurrentNumber.Length > 0 ? double.Parse(State.CurrentNumber) : 0.0;
                        s.Perc = true;
                        switch (s.CurrentOperation)
                        {
                            case "÷":
                                s.Result = s.Number1!.Value / (s.Number2.Value / 100.0) * s.Number1!.Value;
                                break;
                            case "×":
                                s.Result = s.Number1!.Value * (s.Number2.Value / 100.0) * s.Number1!.Value;
                                break;
                            case "+":
                                s.Result = s.Number1!.Value + (s.Number2.Value / 100.0) * s.Number1!.Value;
                                break;
                            case "-":
                                s.Result = s.Number1!.Value - (s.Number2.Value / 100.0) * s.Number1!.Value;
                                break;
                        }
                    }, false);
                }

                break;
            }
            case "+-":
            {
                if (State.CurrentNumber.Length > 0)
                {
                    SetState(
                        s => s.CurrentNumber = s.CurrentNumber.StartsWith('-')
                            ? s.CurrentNumber = s.CurrentNumber[1..]
                            : "-" + s.CurrentNumber, false);
                }

                break;
            }
            case "÷":
            case "×":
            case "+":
            case "-":
            {
                if (State.CurrentOperation.Length == 0 && State.CurrentNumber.Length > 0)
                {
                    SetState(s =>
                    {
                        s.CurrentOperation = key;
                        s.Number1 = double.Parse(s.CurrentNumber);
                        s.CurrentNumber = string.Empty;
                    }, false);
                }

                break;
            }
            default:
                SetState(s => { s.CurrentNumber += key; }, false);
                break;
        }
    }
}

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