// ReSharper disable SpecifyACultureInStringConversionExplicitly
namespace Calculator.Services;
using System.Globalization;

internal class CalculatorEngine
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    
    private decimal? _number1;
    private decimal? _number2;
    private string _currentOperation = string.Empty;
    private decimal _currentValue;
    private bool _isPercentage;
    private bool _calculationJustFinished;
    
    public string CurrentInput { get; private set; } = "0";

    public decimal CurrentValue => _calculationJustFinished ? _currentValue : decimal.Parse(CurrentInput, _culture);

    public string GetExpression()
    {
        if (_number1 == null) return string.Empty;

        var expression = _number1.Value.ToString(_culture);

        if (string.IsNullOrEmpty(_currentOperation)) return string.Empty;

        expression += $" {_currentOperation}";

        if (CurrentInput != "0")
        {
            expression += $" {CurrentInput}";
        }

        return expression;
    }

    public void HandleInput(string key)
    {
        switch (key)
        {
            case "C":
                Clear();
                break;

            case "⌫":
                CurrentInput = CurrentInput.Length > 1 ? CurrentInput[..^1] : "0";
                
                break;

            case "+-":
                if (CurrentInput == "0") break;

                CurrentInput = CurrentInput.StartsWith('-') ? CurrentInput[1..] : "-" + CurrentInput;
                _calculationJustFinished = false;
                break;

            case "." when _calculationJustFinished || !CurrentInput.Contains('.'):
                CurrentInput = _calculationJustFinished ? "0." : $"{CurrentInput}.";
                _calculationJustFinished = false;
                break;

            case "÷" or "×" or "+" or "-":
                if (_number1 != null && !string.IsNullOrEmpty(_currentOperation))
                {
                    Calculate("=");
                }
                else if (_calculationJustFinished)
                {
                    _number1 = _currentValue;  // Use stored decimal for calculation results
                }
                else
                {
                    _number1 = decimal.Parse(CurrentInput, _culture);  // Parse only for user input
                }
                _currentOperation = key;
                CurrentInput = "0";
                _calculationJustFinished = false;
                break;

            case "=" or "%":
                Calculate(key);
                _calculationJustFinished = true;
                break;

            default:
                if (char.IsDigit(key[0]))
                {
                    if (_calculationJustFinished)
                    {
                        CurrentInput = key;
                        _calculationJustFinished = false;
                    }
                    else
                    {
                        CurrentInput = CurrentInput == "0" ? key : CurrentInput + key;
                    }
                }
                break;
        }
    }

    private void Calculate(string key)
    {
        if (_number1 == null || string.IsNullOrEmpty(_currentOperation)) return;

        _number2 = decimal.Parse(CurrentInput, _culture);
        _isPercentage = key == "%";
        var n2 = _isPercentage ? (_number2.Value / 100.0m) * _number1.Value : _number2.Value;

        _currentValue = _currentOperation switch
        {
            "÷" => _number1.Value / n2,
            "×" => _number1.Value * n2,
            "+" => _number1.Value + n2,
            "-" => _number1.Value - n2,
            _ => _number2.Value
        };

        CurrentInput = _currentValue.ToString(_culture);
        _number1 = _currentValue;
        _number2 = null;
        _currentOperation = string.Empty;
        _isPercentage = false;
    }

    private void Clear()
    {
        _number1 = null;
        _number2 = null;
        _currentOperation = string.Empty;
        CurrentInput = "0";
        _currentValue = 0m;
        _isPercentage = false;
        _calculationJustFinished = false;
    }
}