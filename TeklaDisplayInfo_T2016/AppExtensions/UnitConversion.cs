
using System;

namespace AppExtensions
{
  public static class UnitConversion
  {
    public static string MMtoFeetInches(this double mm, int accuracy = 16)
    {
      double value = mm / 25.4;
      return value.ToFeetInchFrac(accuracy);
    }
    public static double M2toSqFt(this double sqm)
    {
      return sqm * 10.7639;
    }

    public static string ToFeetInchFrac(this double Value, int maxDenominator)
    {
      // Calculating the nearest increment of the value
      // argument based on the denominator argument.
      double incValue = Value.RoundToNearest(Math.Pow(maxDenominator, -1));

      // Calculating the remainder of the argument value and the whole value.
      double FeetInch = Math.Truncate(incValue) / 12.0;

      // Calculating the remainder of the argument value and the whole value.
      int Feet = (int)Math.Truncate(FeetInch);

      // Calculating remaining inches.
      incValue -= (double)(Feet * 12.0);

      // Returns the feet plus the remaining amount converted to inch fraction.
      return Feet.ToString() + (char)39 + "-" + incValue.ToInchFrac(maxDenominator);
    }

    public static string ToInchFrac(this double Value, int maxDenominator)
    {
      // Calculating the nearest increment of the value
      // argument based on the denominator argument.
      double incValue = Value.RoundToNearest(Math.Pow(maxDenominator, -1));

      // Identifying the whole number of the argument value.
      int wholeValue = (int)Math.Truncate(incValue);

      // Calculating the remainder of the argument value and the whole value.
      double remainder = incValue - wholeValue;

      // Checking for the whole number case and returning if found.
      if (remainder == 0.0) { return wholeValue.ToString() + (char)34; }

      // Iterating through the exponents of base 2 values until the
      // maximum denominator value has been reached or until the modulus
      // of the divisor.
      for (int i = 1; i < (int)Math.Log(maxDenominator, 2) + 1; i++)
      {
        // Calculating the denominator of the current iteration
        double denominator = Math.Pow(2, i);

        // Calculating the divisor increment value
        double divisor = Math.Pow(denominator, -1);

        // Checking if the current denominator evenly divides the remainder
        if (remainder % divisor == 0.0) // If, yes
        {
          // Calculating the numerator of the remainder 
          // given the calculated denominator
          int numerator = Convert.ToInt32(remainder * denominator);

          // Returning the resulting string from the conversion.
          return wholeValue.ToString() + " " + numerator.ToString() + "/" + ((int)denominator).ToString() + (char)34;
        }
      }
      // Returns Error if something goes wrong.
      return "Error";
    }
    /// <summary>
    /// Rounds the value to the nearest increment. 
    /// Assumes mid-point rounding, value >= 0.5 rounds up, value < 0.5 rounds down.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="increment">Enter the increment value to round toward.</param>
    /// <returns>Returns the value rounded to the nearest increment value.</returns>
    public static double RoundToNearest(this double Value, double increment)
    {
      // Returning the value rounded to the nearest increment value.
      return Math.Round(Value * Math.Pow(increment, -1), 0) * increment;
    }
  }
}


//public static string ToFtInString(this double mm, int accuracy = 16)
//{
//  StringBuilder stringBuilder = new StringBuilder();

//  double num = Math.Abs(mm / 25.4);
//  int num1 = (int)Math.Truncate(num / 12);
//  int num2 = (int)Math.Truncate(num);
//  num -= (double)num2;
//  int num3 = (int)Math.Round(num * accuracy, MidpointRounding.AwayFromZero);
//  int num4 = accuracy;

//  num2 = num2 + num3 / num4;
//  num3 %= num4;
//  num2 %= 12;
//  while (num3 > 1 && num3 % 2 == 0 && num4 > 1 && num4 % 2 == 0)
//  {
//    num3 /= 2;
//    num4 /= 2;
//  }
//  if (num1 == 0 && num2 == 0 && num3 == 0)
//  {
//    return $"0\"";
//  }
//  if (num1 > 0)
//  {
//    stringBuilder.Append($"{num1}'");
//  }
//  if (num2 > 0)
//  {
//    if (num1 == 0)
//    {
//      stringBuilder.Append($"-");
//    }
//    stringBuilder.Append(num2.ToString());
//    if (num3 == 0)
//    {
//      stringBuilder.Append($"\"");
//    }
//    if (num3 > 0)
//    {
//      stringBuilder.Append($" ");
//    }
//  }
//  else if (num1 > 0 && num3 > 0)
//  {
//    stringBuilder.Append($"-");
//  }
//  if (num3 > 0)
//  {
//    stringBuilder.Append(num3.ToString()).Append('/').Append(num4.ToString()).Append($"\"");
//  }

//  return stringBuilder.ToString();
//}