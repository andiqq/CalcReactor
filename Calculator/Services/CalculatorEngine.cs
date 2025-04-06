// ReSharper disable SpecifyACultureInStringConversionExplicitly

using System;

namespace Calculator.Services;
using System.Globalization;

internal class CalculatorEngine
{
    private readonly CultureInfo culture = CultureInfo.InvariantCulture;
    
    public bool IsOverflow { get; set; }
    
    private decimal? number1;
    private decimal? number2;
    private string currentOperation = string.Empty;
    private decimal currentValue;
    private bool isPercentage;
    private bool calculationJustFinished;
    
    public string CurrentInput { get; private set; } = "0";

    public decimal CurrentValue => calculationJustFinished ? currentValue : decimal.Parse(CurrentInput, culture);

    public string GetExpression()
    {
        if (number1 == null) return string.Empty;

        var expression = number1.Value.ToString(culture);

        if (string.IsNullOrEmpty(currentOperation)) return string.Empty;

        expression += $" {currentOperation}";

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
                calculationJustFinished = false;
                break;

            case "." when calculationJustFinished || !CurrentInput.Contains('.'):
                CurrentInput = calculationJustFinished ? "0." : $"{CurrentInput}.";
                calculationJustFinished = false;
                break;

            case "÷" or "×" or "+" or "-":
                if (number1 != null && !string.IsNullOrEmpty(currentOperation))
                {
                    Calculate("=");
                }
                else if (calculationJustFinished)
                {
                    number1 = currentValue;  // Use stored decimal for calculation results
                }
                else
                {
                    number1 = decimal.Parse(CurrentInput, culture);  // Parse only for user input
                }
                currentOperation = key;
                CurrentInput = "0";
                calculationJustFinished = false;
                break;

            case "=" or "%":
                Calculate(key);
                calculationJustFinished = true;
                break;

            default:
                if (char.IsDigit(key[0]))
                {
                    if (calculationJustFinished)
                    {
                        CurrentInput = key;
                        calculationJustFinished = false;
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
        if (number1 == null || string.IsNullOrEmpty(currentOperation)) return;
        
        number2 = decimal.Parse(CurrentInput, culture);
        isPercentage = key == "%";
        var n2 = isPercentage ? (number2.Value / 100.0m) * number1.Value : number2.Value;
        
        // Ignore division by zero
        if (currentOperation == "÷" && n2 == 0) return;

        try
        {
            IsOverflow = false;

            currentValue = currentOperation switch
            {
                "÷" => number1.Value / n2,
                "×" => number1.Value * n2,
                "+" => number1.Value + n2,
                "-" => number1.Value - n2,
                _ => number2.Value
            };
        }
        catch (OverflowException)
        {
            IsOverflow = true;
            calculationJustFinished = true;
        }

        CurrentInput = currentValue.ToString(culture);
        number1 = currentValue;
        number2 = null;
        currentOperation = string.Empty;
        isPercentage = false;
    }

    private void Clear()
    {
        number1 = null;
        number2 = null;
        currentOperation = string.Empty;
        CurrentInput = "0";
        currentValue = 0m;
        isPercentage = false;
        calculationJustFinished = false;
    }
}