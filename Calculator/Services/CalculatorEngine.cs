using System.Security.Cryptography.X509Certificates;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace Calculator.Services;

internal class CalculatorEngine
{
    private decimal? _number1;
    private decimal? _number2;
    private string _currentOperation = string.Empty;
    private decimal _currentValue = 0m;
    private string _currentInput = "0";
    private bool _isPercentage;
    private bool _calculationJustFinished;

    public string CurrentInput => _calculationJustFinished ? FormatNumber(_currentValue) : _currentInput;

    public string GetExpression()
    {
        if (_number1 == null) return string.Empty;

        var expression = FormatNumber(_number1.Value);

        if (string.IsNullOrEmpty(_currentOperation)) return string.Empty;
        
            expression += $" {_currentOperation}";

            if (_currentInput != "0")
            {
                expression += $" {_currentInput}";
            }
        

        if (_isPercentage)
        {
            expression += "%";
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
                if (_currentInput.Length > 1)
                {
                    _currentInput = _currentInput[..^1];
                }
                else
                {
                    _currentInput = "0";
                }
                break;

            case "+-":
                if (_currentInput.StartsWith("-"))
                {
                    _currentInput = _currentInput[1..];
                }
                else
                {
                    _currentInput = "-" + _currentInput;
                }
                break;

            case "." when !_currentInput.Contains('.'):
                _currentInput += ".";
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
                    _number1 = decimal.Parse(_currentInput);  // Parse only for user input
                }
                _currentOperation = key;
                _currentInput = "0";
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
                        _currentInput = key;
                        _calculationJustFinished = false;
                    }
                    else
                    {
                        _currentInput = _currentInput == "0" ? key : _currentInput + key;
                    }
                }
                break;
        }
    }

    private void Calculate(string key)
    {
        if (_number1 == null || string.IsNullOrEmpty(_currentOperation)) return;

        _number2 = decimal.Parse(_currentInput);
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

        _currentInput = FormatNumber(_currentValue);
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
        _currentInput = "0";
        _currentValue = 0m;
        _isPercentage = false;
        _calculationJustFinished = false;
    }

    private static string FormatNumber(decimal number)
    {
        const int maxLength = 10;

        if (Math.Abs((double)number) >= 1e10 || (Math.Abs((double)number) < 0.0001 && number != 0))
        {
            return number.ToString("E4");
        }

        if (number == Math.Floor(number))
        {
            return number.ToString("0");
        }

        string formatted = number.ToString("0.##########").TrimEnd('0').TrimEnd('.');
        if (formatted.Length <= maxLength) return formatted;

        var integerPart = Math.Floor(Math.Abs(number));
        var integerDigits = integerPart.ToString().Length;
        if (number < 0) integerDigits++;

        var decimalPlaces = maxLength - integerDigits - 1;
        if (decimalPlaces > 0)
        {
            return Math.Round(number, decimalPlaces).ToString("0.##########").TrimEnd('0').TrimEnd('.');
        }

        return Math.Round(number).ToString();
    }
}