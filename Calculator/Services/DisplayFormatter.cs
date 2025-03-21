namespace Calculator.Services;

public static class DisplayFormatter
{
    public static (double fontSize, double widthFactor) CalculateFontSize(string text, double baseSize = 63.0)
    {
        var visualWidth = text.Sum(c => c switch
        {
            '1' => 0.6,
            '.' => 0.5,
            '8' => 1.0,
            _ => 0.95
        });

        return visualWidth > 9.0
            ? (baseSize * 9.0 / visualWidth, visualWidth / 9.0)
            : (baseSize, 1.0);
    }

    public static string FormatNumber(decimal number)
    {
        if (Math.Abs((double)number) >= 1e10 || (Math.Abs((double)number) < 0.0001 && number != 0))
        {
            return number.ToString("E4");
        }

        if (number == Math.Floor(number))
        {
            return number.ToString("0");
        }

        var formatted = number.ToString("0.##########").TrimEnd('0').TrimEnd('.');
        var (_, widthFactor) = CalculateFontSize(formatted);

        if (widthFactor <= 1.0) return formatted;

        var significantDigits = (int)(16 / widthFactor);
        return Math.Round(number, Math.Max(0, significantDigits))
            .ToString("0.##########")
            .TrimEnd('0')
            .TrimEnd('.');
    }
}