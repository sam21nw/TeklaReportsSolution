using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

using InfoDisplay_2016;

namespace UtilityExtensions
{
  /// <summary>
  /// Extensions class for Tekla.BIM.Quantities class
  /// </summary>
  public static class TsMass
  {
    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 if XS_IMPERIAL=TRUE, mm to decimal places otherwise
    /// </summary>
    public static string ToCurrentUnits(this Mass ln)
    {
      return new Model().IsImperial() ? ln.ToString(MassUnit.Kilogram, "0.000") : ln.ToString(MassUnit.Pound, "0.000");
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
      var mass = Mass.Parse(str, new Model().IsImperial() ? MassUnit.Pound : MassUnit.Kilogram);
      return mass.Kilograms;
    }

    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 in
    /// </summary>
    public static string ToImperialUnits(this Mass mass)
    {
      return mass.ToString(MassUnit.Pound, "0.000");
    }

    /// <summary>
    /// Returns mm to decimal places string
    /// </summary>
    public static string ToMetricUnits(this Mass mass)
    {
      return mass.ToString(MassUnit.Kilogram, "0.000");
    }
  }
}
