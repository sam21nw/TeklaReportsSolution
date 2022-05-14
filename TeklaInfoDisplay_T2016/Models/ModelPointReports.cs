
using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

using TeklaInfoDisplay.UtilityExtensions;

using UtilityExtensions;

namespace TeklaInfoDisplay.Models
{
  internal static class ModelControlObjectReports
  {
    public static string GetPointCoords(this ControlPoint pt)
    {
      var dx = pt.Point.X;
      var dy = pt.Point.Y;
      var dz = pt.Point.Z;

      var ImpdX = new Length(dx).ToImperialUnits();
      var ImpdY = new Length(dy).ToImperialUnits();
      var ImpdZ = new Length(dz).ToImperialUnits();

      return string.Format($"Selected Point Coordinates:-  [{ImpdX}, {ImpdY}, {ImpdZ}] ft-in    ({dx.ToString("#")}, {dy.ToString("#")}, {dz.ToString("#")}) mm");
    }
  }
}
