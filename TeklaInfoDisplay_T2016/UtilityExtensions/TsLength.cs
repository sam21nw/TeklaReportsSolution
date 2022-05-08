using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

using InfoDisplay_2016;

namespace UtilityExtensions
{
  /// <summary>
  /// Extensions class for Tekla.BIM.Quantities class
  /// </summary>
  public static class TsLength
  {
    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 if XS_IMPERIAL=TRUE, mm to decimal places otherwise
    /// </summary>
    public static string ToCurrentUnits(this Length ln)
    {
      return new Model().IsImperial() ? ln.ToString(LengthUnit.Foot, "1/16") : ln.ToString(LengthUnit.Millimeter, "0.00");
    }

    /// <summary>
    /// Uses BIM Length.Parse to convert string to Length and returns Millimeter value
    /// Uses XS_IMPERIAL to decide if input is Foot versus millimeter value in string format
    /// May not work if input is other than foot or mm and flag not set
    /// </summary>
    /// <param name="str">Numberic value in string format</param>
    /// <returns>Double millimeter value</returns>
    public static double FromCurrrentUnits(this string str)
    {
      var length = Length.Parse(str, new Model().IsImperial() ? LengthUnit.Inch : LengthUnit.Millimeter);
      return length.Millimeters;
    }

    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 in
    /// </summary>
    public static string ToImperialUnits(this Length ln)
    {
      return ln.ToString(LengthUnit.Foot, "1/16");
    }

    /// <summary>
    /// Returns mm to decimal places string
    /// </summary>
    public static string ToMetricUnits(this Length ln)
    {
      return ln.ToString(LengthUnit.Millimeter, "0");
    }
  }
}
