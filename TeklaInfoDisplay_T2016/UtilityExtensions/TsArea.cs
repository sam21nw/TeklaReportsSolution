﻿using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

namespace UtilityExtensions
{
  /// <summary>
  /// Extensions class for Tekla.BIM.Quantities class
  /// </summary>
  public static class TsArea
  {
    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 if XS_IMPERIAL=TRUE, mm to decimal places otherwise
    /// </summary>
    public static string ToCurrentUnits(this Area ln)
    {
      return new Model().IsImperial() ? ln.ToString(AreaUnit.SquareMeter, "0.000") : ln.ToString(AreaUnit.SquareFoot, "0.000");
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
      var area = Area.Parse(str, new Model().IsImperial() ? AreaUnit.SquareFoot : AreaUnit.SquareMeter);
      return area.SquareMillimeters;
    }

    /// <summary>
    /// Returns ft-fraction inch string rounded to 1/16 in
    /// </summary>
    public static string ToImperialUnits(this Area ar)
    {
      return ar.ToString(AreaUnit.SquareFoot, "0.000");
    }

    /// <summary>
    /// Returns mm to decimal places string
    /// </summary>
    public static string ToMetricUnits(this Area ar)
    {
      return ar.ToString(AreaUnit.SquareMeter, "0.000");
    }
  }
}
