using System;
using System.Collections.Generic;
using System.Linq;

using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

using UtilityExtensions;

namespace InfoDisplay_2019
{
  public static class ModelPartReports
  {
    private static readonly Model _model = new Model();
    public static string GetDistanceBtwParts(List<Beam> selParts)
    {
      double cogX = 0;
      double cogY = 0;

      List<double> opXdists = new List<double>(2);
      List<double> opYdists = new List<double>(2);

      foreach (var part in selParts)
      {
        part.GetReportProperty("COG_X", ref cogX);
        part.GetReportProperty("COG_Y", ref cogY);

        opXdists.Add(cogX);
        opYdists.Add(cogY);
      }

      double Xdist = Math.Abs(opXdists.Last() - opXdists.First());
      double Ydist = Math.Abs(opYdists.Last() - opYdists.First());

      var XdistMetric = new Length(Xdist).ToMetricUnits();
      var YdistMetric = new Length(Ydist).ToMetricUnits();

      var XdistImperial = new Length(Xdist).ToImperialUnits();
      var YdistImperial = new Length(Ydist).ToImperialUnits();

      string result = $"Distance b/w selected:-   [X:{XdistMetric} ({XdistImperial})]  [Y:{YdistMetric} ({YdistImperial})]";

      return result;
    }
    public static string GetSelectedObjectReport(ModelObject obj)
    {
      string contentType = string.Empty;
      string objectName = string.Empty;
      string mainPartName = string.Empty;
      string partPrefix = string.Empty;
      string assPrefix = string.Empty;
      string partPos = string.Empty;
      string assPos = string.Empty;
      string topLevel = string.Empty;
      string bottomLevel = string.Empty;
      string profile = string.Empty;
      string userPhase = string.Empty;
      string userField4 = string.Empty;
      string userField3 = string.Empty;
      string userField2 = string.Empty;
      string userField1 = string.Empty;
      double netArea = 0;
      double grossArea = 0;
      double grossWeight = 0;
      double netWeight = 0;
      double suppPartWeight = 0;
      string boltStandard = string.Empty;
      string weldType1 = string.Empty;
      string weldType2 = string.Empty;

      int modelTotal = 0;
      double length = 0;
      double width = 0;
      double height = 0;
      double flangeThickness = 0;
      double diameter = 0;
      double volume = 0;
      double cogX = 0;
      double cogY = 0;
      double weldSize1 = 0;
      double weldSize2 = 0;

      string result;

      obj.GetReportProperty("CONTENTTYPE", ref contentType);
      obj.GetReportProperty("NAME", ref objectName);
      obj.GetReportProperty("MAINPART.NAME", ref mainPartName);
      obj.GetReportProperty("PART_PREFIX", ref partPrefix);
      obj.GetReportProperty("ASSEMBLY_PREFIX", ref assPrefix);
      obj.GetReportProperty("PART_POS", ref partPos);
      obj.GetReportProperty("ASSEMBLY_POS", ref assPos);
      obj.GetReportProperty("TOP_LEVEL_GLOBAL", ref topLevel);
      obj.GetReportProperty("BOTTOM_LEVEL_GLOBAL", ref bottomLevel);
      obj.GetReportProperty("PROFILE", ref profile);
      obj.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
      obj.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
      obj.GetReportProperty("LENGTH", ref length);
      obj.GetReportProperty("HEIGHT", ref width);
      obj.GetReportProperty("WIDTH", ref height);
      obj.GetReportProperty("WEIGHT_NET", ref netWeight);
      obj.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
      obj.GetReportProperty("PROFILE.FLANGE_THICKNESS", ref flangeThickness);
      obj.GetReportProperty("ASSEMBLY.SUPPLEMENT_PART_WEIGHT", ref suppPartWeight);
      obj.GetReportProperty("USER_PHASE", ref userPhase);
      obj.GetReportProperty("USER_FIELD_4", ref userField4);
      obj.GetReportProperty("USER_FIELD_3", ref userField3);
      obj.GetReportProperty("USER_FIELD_2", ref userField2);
      obj.GetReportProperty("USER_FIELD_1", ref userField1);
      obj.GetReportProperty("MODEL_TOTAL", ref modelTotal);
      obj.GetReportProperty("DIAMETER", ref diameter);
      obj.GetReportProperty("BOLT_STANDARD", ref boltStandard);
      obj.GetReportProperty("VOLUME_NET", ref volume);
      obj.GetReportProperty("COG_X", ref cogX);
      obj.GetReportProperty("COG_Y", ref cogY);
      obj.GetReportProperty("WELD_SIZE1", ref weldSize1);
      obj.GetReportProperty("WELD_SIZE2", ref weldSize2);
      obj.GetReportProperty("WELD_TYPE1", ref weldType1);
      obj.GetReportProperty("WELD_TYPE2", ref weldType2);

      double weightDiff = Math.Round(grossWeight - netWeight, 3);

      double cutOutArea = Math.Round((grossArea - netArea) / 1000000, 3);

      string ps = weightDiff > 0 || suppPartWeight > 0 ? "Shp" : "Pln";
      double gratingArea = (weightDiff == 0) ? netArea : grossArea;

      var gratingAreaMetric = new Area(gratingArea).ToMetricUnits();
      var gratingAreaImperial = new Area(gratingArea).ToImperialUnits();

      var grossWeightMetric = new Mass(grossWeight).ToMetricUnits();
      var grossWeightImperial = new Mass(grossWeight).ToImperialUnits();

      string dispCogX = new Length(cogX).ToMetricUnits();
      string dispCogY = new Length(cogY).ToMetricUnits();

      if (_model.IsImperial())
      {
        var ImpCogX = new Length(cogX).ToImperialUnits();
        var ImpCogY = new Length(cogY).ToImperialUnits();

        dispCogX = $"{ImpCogX}";
        dispCogY = $"{ImpCogY}";
      }

      var lengthMetric = new Length(length).ToMetricUnits();
      var lengthImperial = new Length(length).ToImperialUnits();
      var widthMetric = new Length(width).ToMetricUnits();
      var widthImperial = new Length(width).ToImperialUnits();
      var heightMetric = new Length(height).ToMetricUnits();
      var heightImperial = new Length(height).ToImperialUnits();

      if (objectName == string.Empty)
      {
        objectName = $"{contentType}";
      }

      var uF4Strs = (userField4.Length != 0) ? $", U/F4:{userField4}" : "";
      var uF3Strs = (userField3.Length != 0) ? $", U/F3:{userField3}" : "";
      var uF2Strs = (userField2.Length != 0) ? $", U/F2:{userField2}" : "";
      var uF1Strs = (userField1.Length != 0) ? $", U/F1:{userField1}" : "";

      var userStrs = $"U/Ph:{userPhase}{uF4Strs}{uF3Strs}{uF2Strs}{uF1Strs}";

      if (contentType == "PART")
      {
        if (objectName.ToUpper().Contains("GRAT") || partPrefix.ToUpper().Contains("GR"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{partPos}/{assPos}]  [Nos:{modelTotal}]   [{profile}, {widthMetric} ({widthImperial})]  [Span: {lengthMetric} ({lengthImperial})]  [Area: {gratingAreaMetric} ({gratingAreaImperial}), cut:{cutOutArea}, P/S:{ps}]  [Wt: {grossWeightMetric} ({grossWeightImperial})]  [{userStrs}]";
        }
        else if (objectName.ToUpper().Contains("STIFFENER_ANGLE") || partPrefix.ToUpper().Contains("SA"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{partPos}/{assPos}]  [Nos:{modelTotal}]  [{profile}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]  [U/Ph:{userPhase}, U/F4:{userField4}]";
        }
        else if (objectName.ToUpper().Contains("CHEQ") || partPrefix.ToUpper().Contains("CPL") || partPrefix.ToUpper().Contains("CHQ"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{partPos}/{assPos}]  [Nos:{modelTotal}]   [{profile}, {widthMetric} ({widthImperial})]  [Span: {lengthMetric} ({lengthImperial})]  [Area: {gratingAreaMetric} ({gratingAreaImperial}), cut:{cutOutArea}, P/S:{ps}]  [Wt: {grossWeightMetric} ({grossWeightImperial})]  [{userStrs}]";
        }
        else if (partPrefix.ToUpper().Contains("TP") || partPrefix.ToUpper().Contains("BB") || partPrefix.ToUpper().Contains("NS") || partPrefix.ToUpper().Contains("SB") || objectName.ToUpper().Contains("TOE") || objectName.ToUpper().Contains("NOS") || objectName.ToUpper().Contains("BIND") || objectName.ToUpper().Contains("CHEQ") || objectName.ToUpper().Contains("SUPP"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{partPos}/{assPos}]  [{profile}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]";
        }
        else if (objectName.ToUpper().Contains("BE") || profile.Contains("UP"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{profile}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]  [FlangeThk:{string.Format("{0:0.0}", flangeThickness)} mm]";
        }
        else if (objectName.ToUpper().Contains("COL"))
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{profile}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]   Coordinates:[X:{dispCogX}, Y:{dispCogY}]";
        }
        else if (objectName.ToUpper().Contains("OP"))
        {
          string opngProfileDisp;
          if (_model.IsImperial())
          {
            if (profile.ToUpper().Contains("D"))
            {
              opngProfileDisp = $"D{widthImperial}";
            }
            else
            {
              opngProfileDisp = $"{widthImperial}X{heightImperial}";
            }
          }
          else
          {
            if (profile.ToUpper().Contains("D"))
            {
              opngProfileDisp = $"D{widthMetric}";
            }
            else
            {
              opngProfileDisp = $"{widthMetric}X{heightMetric}";
            }
          }

          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{opngProfileDisp}]   Coordinates:[X:{dispCogX}, Y:{dispCogY}]";
        }
        else
        {
          result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{profile}]  [Wt:{grossWeightMetric} ({grossWeightImperial})]";
        }
      }
      else if (contentType == "BOLT" || contentType == "NUT")
      {
        result = $"{objectName}  [M{new Length(diameter).ToMetricUnits()} X {lengthMetric}]  [{boltStandard}]";
      }
      else if (contentType == "HOLE")
      {
        result = $"{objectName}  [Dia:{new Length(diameter).ToMetricUnits()}, Length:{lengthMetric}]";
      }
      else if (contentType == "ASSEMBLY")
      {
        if (mainPartName.ToUpper().Contains("GRAT"))
        {
          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]  [{assPos}]  [Nos:{modelTotal}]   [Width: {widthMetric} ({widthImperial})]  [Span: {lengthMetric} ({lengthImperial})]   [Area:{gratingAreaMetric} ({gratingAreaImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]  [U/Ph:{userPhase}, U/F4:{userField4}]";
        }
        else if (mainPartName.ToUpper().Contains("STIFFENER_ANGLE") || partPrefix.ToUpper().Contains("SA"))
        {
          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]  [{assPos}]  [Nos:{modelTotal}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]  [U/Ph:{userPhase}, U/F4:{userField4}]";
        }
        else if (mainPartName.ToUpper().Contains("BE") || mainPartName.ToUpper().Contains("JO") || mainPartName.ToUpper().Contains("BRACE") || profile.Contains("UP"))
        {
          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]";
        }
        else if (mainPartName.ToUpper().Contains("COL"))
        {
          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]   [Length:{lengthMetric} ({lengthImperial})]  [Wt:{grossWeightMetric} ({grossWeightImperial})]   Coordinates:[X:{dispCogX}, Y:{dispCogY}]";
        }
        else if (mainPartName.ToUpper().Contains("OP"))
        {
          string opngProfileDisp;
          if (_model.IsImperial())
          {
            if (profile.ToUpper().Contains("D"))
            {
              opngProfileDisp = $"D{widthImperial}";
            }
            else
            {
              opngProfileDisp = $"{widthImperial}X{heightImperial}";
            }
          }
          else
          {
            if (profile.ToUpper().Contains("D"))
            {
              opngProfileDisp = $"D{widthMetric}";
            }
            else
            {
              opngProfileDisp = $"{widthMetric}X{heightMetric}";
            }
          }

          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]  [{opngProfileDisp}]   Coordinates:[X:{dispCogX}, Y:{dispCogY}]";
        }
        else
        {
          result = $"{mainPartName}  [{topLevel}/{bottomLevel}]  [Wt:{grossWeightMetric} ({grossWeightImperial})]";
        }
      }
      else if (contentType == "WELD")
      {
        var type1 = string.Empty;
        var type2 = string.Empty;

        if (weldType1 == "W10")
        {
          type1 = "Fillet";
        }
        else if (weldType1 == "W11")
        {
          type1 = "Plug";
        }
        else if (weldType1 == "W12")
        {
          type1 = "Spot";
        }
        else if (weldType1 == "W13")
        {
          type1 = "Seam";
        }
        else if (weldType1 == "W14")
        {
          type1 = "Slot";
        }
        else if (weldType1 == "W15")
        {
          type1 = "Flare_bevel_groove";
        }
        else if (weldType1 == "W16")
        {
          type1 = "Flare_V_groove";
        }
        else if (weldType1 == "W17")
        {
          type1 = "Corner_flange";
        }
        else if (weldType1 == "W18")
        {
          type1 = "Single_bevel";
        }
        else if (weldType1 == "W19")
        {
          type1 = "Square_groove_plus_fillet";
        }
        else if (weldType1 == "None")
        {
          type1 = "None";
        }

        if (weldType2 == "W10")
        {
          type2 = "Fillet";
        }
        else if (weldType2 == "W11")
        {
          type2 = "Plug";
        }
        else if (weldType2 == "W12")
        {
          type2 = "Spot";
        }
        else if (weldType2 == "W13")
        {
          type2 = "Seam";
        }
        else if (weldType2 == "W14")
        {
          type2 = "Slot";
        }
        else if (weldType2 == "W15")
        {
          type2 = "Flare_bevel_groove";
        }
        else if (weldType2 == "W16")
        {
          type2 = "Flare_V_groove";
        }
        else if (weldType2 == "W17")
        {
          type2 = "Corner_flange";
        }
        else if (weldType2 == "W18")
        {
          type2 = "Single_bevel";
        }
        else if (weldType2 == "W19")
        {
          type2 = "Square_groove_plus_fillet";
        }
        else if (weldType2 == "None")
        {
          type2 = "None";
        }

        result = $"{contentType}  A/B[{type1}/{type2}, {weldSize1}/{weldSize2}]";
      }
      else if (contentType.ToUpper().Contains("CAST_UNIT") || objectName.ToUpper().Contains("CONC") || objectName.ToUpper().Contains("WALL"))
      {
        result = $"{objectName}  [{topLevel}/{bottomLevel}]  [Volume:{Math.Round(volume, 3)} cu.m]";
      }
      else if (objectName.ToUpper().Contains("COORD") || objectName.ToUpper().Contains("SKIRT"))
      {
        result = $"{objectName}   Coordinates:[X:{dispCogX}, Y:{dispCogY}]";
      }
      else if (objectName.ToUpper().Contains("DUM"))
      {
        result = $"{objectName}  [{topLevel}/{bottomLevel}]  [{profile}]";
      }
      else
      {
        result = $"{objectName}";
      }

      return result;
    }
    public static string GetPartsGroupReport(List<ModelObject> parts)
    {
      string name = string.Empty;
      string nameStr = string.Empty;

      double length = 0;
      double grossWeight = 0;
      double totalPartLength = 0;
      double totalSubPartWeight = 0;
      string profile = string.Empty;

      List<string> uniqueProfile = new List<string>();
      IEnumerable<string> distinctProfiles = null;

      string unqProfileStr = string.Empty;

      int totalPartNos = 0;

      foreach (var part in parts)
      {
        part.GetReportProperty("NAME", ref name);
        part.GetReportProperty("LENGTH", ref length);
        part.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
        part.GetReportProperty("PROFILE", ref profile);

        nameStr = name;
        uniqueProfile.Add(profile);

        distinctProfiles = uniqueProfile.Distinct();

        totalPartNos += 1;
        totalPartLength += length;
        totalSubPartWeight += grossWeight;
      }

      foreach (var item in distinctProfiles)
      {
        unqProfileStr += $" {item} ";
        if (unqProfileStr.Split(' ').Length > 1)
        {
          nameStr = $"PART";
        }
        else
        {
          nameStr = name;
        }
      }

      var totalPartLengthImperial = new Length(totalPartLength).ToImperialUnits();
      var totalSubPartWeightPounds = new Mass(totalSubPartWeight).ToImperialUnits();

      string result = $"Selected {nameStr}s summary:-  [Nos:{totalPartNos}]  [{unqProfileStr}]   [Length:{Math.Round(totalPartLength / 1000, 3)} m ({totalPartLengthImperial})]  [Wt:{Math.Round(totalSubPartWeight, 3)} kg ({totalSubPartWeightPounds})]";

      return result;
    }
  }
}
