using AppTheme = Calculator.Resources.Styles.AppTheme;

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
        };
        switch (key)
        {
            case "back":
            {
                if (State.CurrentNumber.Length <= 0) break;
                SetState(s => s.CurrentNumber = s.CurrentNumber.Substring(0, s.CurrentNumber.Length - 1), false);
                break;
            }
            case ".":
            {
                if (State.CurrentNumber.Length <= 0 || State.CurrentNumber.Contains('.')) break;
                SetState(s => s.CurrentNumber += key, false);
                break;
            }
            case "0":
            {
                if (State.CurrentNumber.Length <= 0) break;
                SetState(s => s.CurrentNumber += key, false);
                break;
            }
            case "C":
                SetState(s => s.CurrentNumber = string.Empty);
                break;
            case "=":
            {
                if (State.CurrentOperation.Length <= 0 || State.Number1 == null) break;

                SetState(s =>
                {
                    s.Number2 = State.CurrentNumber.Length > 0 ? double.Parse(State.CurrentNumber) : 0.0;
                    s.Result = s.CurrentOperation switch
                    {
                        "÷" => s.Number1!.Value / s.Number2.Value,
                        "×" => s.Number1!.Value * s.Number2.Value,
                        "+" => s.Number1!.Value + s.Number2.Value,
                        "-" => s.Number1!.Value - s.Number2.Value,
                        _ => s.Result
                    };
                }, false);

                break;
            }
            case "%":
            {
                if (State.CurrentOperation.Length <= 0 || State.Number1 == null) break;

                SetState(s =>
                {
                    s.Number2 = State.CurrentNumber.Length > 0 ? double.Parse(State.CurrentNumber) : 0.0;
                    s.Perc = true;
                    s.Result = s.CurrentOperation switch
                    {
                        "÷" => s.Number1!.Value / (s.Number2.Value / 100.0) * s.Number1!.Value,
                        "×" => s.Number1!.Value * (s.Number2.Value / 100.0) * s.Number1!.Value,
                        "+" => s.Number1!.Value + (s.Number2.Value / 100.0) * s.Number1!.Value,
                        "-" => s.Number1!.Value - (s.Number2.Value / 100.0) * s.Number1!.Value,
                        _ => s.Result
                    };
                }, false);
                
                break;
            }
            case "+-":
            {
                if (State.CurrentNumber.Length <= 0) break;
                SetState(
                        s => s.CurrentNumber = s.CurrentNumber.StartsWith('-')
                            ? s.CurrentNumber = s.CurrentNumber[1..]
                            : "-" + s.CurrentNumber, false);
                }
                break;
            case "÷":
            case "×":
            case "+":
            case "-":
            {
                if (State.CurrentOperation.Length != 0 || State.CurrentNumber.Length <= 0) break;
                    SetState(s =>
                    {
                        s.CurrentOperation = key;
                        s.Number1 = double.Parse(s.CurrentNumber);
                        s.CurrentNumber = string.Empty;
                    }, false);
                break;
            }
            default:
                SetState(s => { s.CurrentNumber += key; }, false);
                break;
        }
    }
}