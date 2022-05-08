using System;
using System.Collections.Generic;
using System.Linq;

using Tekla.BIM.Quantities;
using Tekla.Structures.Model;

using UtilityExtensions;

namespace InfoDisplay_2019
{
  internal class GratingReport
  {
    public static string GetGratingGroupReport(List<Beam> gratings)
    {
      string partPos = string.Empty;
      string assPos = string.Empty;
      string userPhase = string.Empty;
      string userField4 = string.Empty;
      string userField3 = string.Empty;
      string userField2 = string.Empty;
      string userField1 = string.Empty;
      double netArea = 0;
      double grossArea = 0;
      double netWeight = 0;
      double grossWeight = 0;
      double suppPartWeight = 0;
      double gratingArea = 0;
      double gratingAreaWHare = 0;

      int totalGratingNos = 0;
      int totalShapePanels = 0;
      int totalPlainPanels = 0;
      double assWeight = 0;
      int uniqueGrNos = 0;
      int uniqueShpNos = 0;
      int uniquePlnNos = 0;
      double totalGratingArea = 0;
      double totalGratingAreaWHare = 0;
      double totalGratingWeight = 0;
      double totalGratingAssWeight = 0;
      string uniqueGrPos = string.Empty;
      string uniqueShpPos = string.Empty;
      string uniquePlnPos = string.Empty;

      string uPhStr = string.Empty;
      string uF4Str = string.Empty;
      string uF3Str = string.Empty;
      string uF2Str = string.Empty;
      string uF1Str = string.Empty;

      List<string> uniqueUph = new List<string>();
      List<string> uniqueUf4 = new List<string>();
      List<string> uniqueUf3 = new List<string>();
      List<string> uniqueUf2 = new List<string>();
      List<string> uniqueUf1 = new List<string>();

      foreach (var grating in gratings)
      {
        grating.GetReportProperty("PART_POS", ref partPos);
        grating.GetReportProperty("ASSEMBLY_POS", ref assPos);
        grating.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
        grating.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
        grating.GetReportProperty("WEIGHT_NET", ref netWeight);
        grating.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
        grating.GetReportProperty("ASSEMBLY.WEIGHT_GROSS", ref assWeight);
        grating.GetReportProperty("ASSEMBLY.SUPPLEMENT_PART_WEIGHT", ref suppPartWeight);
        grating.GetReportProperty("USER_PHASE", ref userPhase);
        grating.GetReportProperty("USER_FIELD_4", ref userField4);
        grating.GetReportProperty("USER_FIELD_3", ref userField3);
        grating.GetReportProperty("USER_FIELD_2", ref userField2);
        grating.GetReportProperty("USER_FIELD_1", ref userField1);

        double weightDiff = Math.Round(grossWeight - netWeight, 3);
        double cutOutArea = Math.Round((grossArea - netArea) / 1000000, 3);
        string ps = weightDiff > 0 || suppPartWeight > 0 ? "Shp" : "Pln";
        gratingArea = (weightDiff == 0) ? netArea : grossArea;
        gratingAreaWHare = (cutOutArea > 0.2) ? netArea : grossArea;

        totalGratingNos += 1;
        totalGratingArea += gratingArea;
        totalGratingAreaWHare += gratingAreaWHare;
        totalGratingWeight += grossWeight;
        totalGratingAssWeight += assWeight;

        if (!uniqueGrPos.Split(' ').Contains(partPos))
        {
          uniqueGrPos += $"{partPos} ";
          uniqueGrNos += 1;
        }
        if (ps == "Shp")
        {
          totalShapePanels += 1;
          if (!uniqueShpPos.Split(' ').Contains(partPos))
          {
            uniqueShpPos += $"{partPos} ";
            uniqueShpNos += 1;
          }
        }
        else
        {
          totalPlainPanels += 1;
          if (!uniquePlnPos.Split(' ').Contains(partPos))
          {
            uniquePlnPos += $"{partPos} ";
            uniquePlnNos += 1;
          }
        }

        uniqueUph.Add(userPhase);
        uniqueUf4.Add(userField4);
        uniqueUf3.Add(userField3);
        uniqueUf2.Add(userField2);
        uniqueUf1.Add(userField1);
      }
      var totalGratingAreaMetric = new Area(totalGratingArea).ToMetricUnits();
      var totalGratingWHareAreaMetric = new Area(totalGratingAreaWHare).ToMetricUnits();
      var totalGratingWeightKgs = new Mass(totalGratingWeight).ToMetricUnits();
      var totalGratingAssWeightKgs = new Mass(totalGratingAssWeight).ToMetricUnits();

      var totalGratingAreaSqFt = new Area(totalGratingArea).ToImperialUnits();
      var totalGratingWHareAreaSqFt = new Area(totalGratingAreaWHare).ToImperialUnits();
      var totalGratingWeightPounds = new Mass(totalGratingWeight).ToImperialUnits();
      var totalGratingAssWeightPounds = new Mass(totalGratingAssWeight).ToImperialUnits();

      IEnumerable<string> distinctUphs = uniqueUph.Distinct();
      IEnumerable<string> distinctUf4s = uniqueUf4.Distinct();
      IEnumerable<string> distinctUf3s = uniqueUf3.Distinct();
      IEnumerable<string> distinctUf2s = uniqueUf2.Distinct();
      IEnumerable<string> distinctUf1s = uniqueUf1.Distinct();

      string[] distinctUphsArrSorted = distinctUphs.ToArray();
      string[] distinctUf4sArrSorted = distinctUf4s.ToArray();
      string[] distinctUf3sArrSorted = distinctUf3s.ToArray();
      string[] distinctUf2sArrSorted = distinctUf2s.ToArray();
      string[] distinctUf1sArrSorted = distinctUf1s.ToArray();

      Array.Sort(distinctUphsArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf4sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf3sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf2sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf1sArrSorted, new AlphanumComparator());

      foreach (var item in distinctUphsArrSorted)
      {
        uPhStr += $"{item} ";
      }
      foreach (var item in distinctUf4sArrSorted)
      {
        uF4Str += $"{item} ";
      }
      foreach (var item in distinctUf3sArrSorted)
      {
        uF3Str += $"{item} ";
      }
      foreach (var item in distinctUf2sArrSorted)
      {
        uF2Str += $"{item} ";
      }
      foreach (var item in distinctUf1sArrSorted)
      {
        uF1Str += $"{item} ";
      }
      var uF4Strs = string.Empty;
      var uF3Strs = string.Empty;
      var uF2Strs = string.Empty;
      var uF1Strs = string.Empty;

      if (distinctUf4sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF4Strs = "";
      }
      else
      {
        uF4Strs = $", U/F4s:{uF4Str}";
      }
      if (distinctUf3sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF3Strs = "";
      }
      else
      {
        uF3Strs = $", U/F3s:{uF3Str}";
      }
      if (distinctUf2sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF2Strs = "";
      }
      else
      {
        uF2Strs = $", U/F2s:{uF2Str}";
      }
      if (distinctUf1sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF1Strs = "";
      }
      else
      {
        uF1Strs = $", U/F1s:{uF1Str}";
      }

      var userStrs = $"U/Phs:{uPhStr}{uF4Strs}{uF3Strs}{uF2Strs}{uF1Strs}";

      string result = $"Grating summary:- [Nos(Unq):{totalGratingNos}({uniqueGrNos}), Shp:{totalShapePanels}({uniqueShpNos}), Pln:{totalPlainPanels}({uniquePlnNos})]  [Ar:{totalGratingAreaMetric} ({totalGratingAreaSqFt})]  [W/HAr:{totalGratingWHareAreaMetric} ({totalGratingWHareAreaSqFt})]  [Gr.Wt:{totalGratingWeightKgs} ({totalGratingWeightPounds})]  [As.Wt:{totalGratingAssWeightKgs} ({totalGratingAssWeightPounds})]  [{userStrs}]";
      return result;
    }
    public static string GetGratingAssGroupReport(List<Assembly> gratingAssemblies)
    {
      string assName = string.Empty;
      int totalGratingAssNos = 0;
      double grossArea = 0;
      double netArea = 0;
      double grossWeight = 0;
      double netWeight = 0;
      double totalGratingArea = 0;
      double totalGratingAreaWHare = 0;
      double totalGratingAssWeight = 0;

      foreach (var gratingAssembly in gratingAssemblies)
      {
        gratingAssembly.GetReportProperty("NAME", ref assName);
        gratingAssembly.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
        gratingAssembly.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
        gratingAssembly.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
        gratingAssembly.GetReportProperty("WEIGHT_NET", ref netWeight);

        double weightDiff = Math.Round(grossWeight - netWeight, 3);
        double cutOutArea = Math.Round((grossArea - netArea) / 1000000, 3);

        double gratingArea = (weightDiff == 0) ? netArea : grossArea;
        double gratingAreaWHare = (cutOutArea > 0.2) ? netArea : grossArea;
        totalGratingAssNos += 1;
        totalGratingArea += gratingArea;
        totalGratingAssWeight += grossWeight;
        totalGratingAreaWHare += gratingAreaWHare;
      }
      var totalGratingAreaMetric = new Area(totalGratingArea).ToMetricUnits();
      var totalGratingWHareAreaMetric = new Area(totalGratingAreaWHare).ToMetricUnits();
      var totalGratingAssWeightKgs = new Mass(totalGratingAssWeight).ToMetricUnits();

      var totalGratingAreaSqFt = new Area(totalGratingArea).ToImperialUnits();
      var totalGratingWHareAreaSqFt = new Area(totalGratingAreaWHare).ToImperialUnits();
      var totalGratingAssWeightPounds = new Mass(totalGratingAssWeight).ToImperialUnits();

      string result = $"Grating Assembly summary:-  [Nos:{totalGratingAssNos}]   [Ar:{totalGratingAreaMetric} ({totalGratingAreaSqFt})]  [W/HAr:{totalGratingWHareAreaMetric} ({totalGratingWHareAreaSqFt})]  [As.Wt:{totalGratingAssWeightKgs} ({totalGratingAssWeightPounds})]";

      return result;
    }
    public static string GetGratingPartsGroupReport(List<ModelObject> parts)
    {
      string name = string.Empty;

      double length = 0;
      double grossWeight = 0;
      double totalPartLength = 0;
      double totalSubPartWeight = 0;
      string profile = string.Empty;

      List<string> uniqueProfile = new List<string>();
      string unqProfileStr = string.Empty;

      int totalPartNos = 0;

      foreach (var part in parts)
      {
        part.GetReportProperty("NAME", ref name);
        part.GetReportProperty("LENGTH", ref length);
        part.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
        part.GetReportProperty("PROFILE", ref profile);

        uniqueProfile.Add(profile);


        totalPartNos += 1;
        totalPartLength += length;
        totalSubPartWeight += grossWeight;
      }
      IEnumerable<string> distinctProfiles = uniqueProfile.Distinct();
      foreach (var item in distinctProfiles)
      {
        unqProfileStr += $" {item} ";
      }

      var totalPartLengthImperial = new Length(totalPartLength).ToImperialUnits();
      var totalSubPartWeightPounds = new Mass(totalSubPartWeight).ToImperialUnits();

      string result = $"Selected {name}s summary:-  [Nos:{totalPartNos}]  [{unqProfileStr}]   [Length:{Math.Round(totalPartLength / 1000, 3)} m ({totalPartLengthImperial})]  [Wt:{Math.Round(totalSubPartWeight, 3)} kg ({totalSubPartWeightPounds})]";

      return result;
    }
    public static string GetCheqPLGroupReport(List<Beam> cpls)
    {
      string partPos = string.Empty;
      string assPos = string.Empty;
      string userPhase = string.Empty;
      string userField4 = string.Empty;
      string userField3 = string.Empty;
      string userField2 = string.Empty;
      string userField1 = string.Empty;
      double netArea = 0;
      double grossArea = 0;
      double netWeight = 0;
      double grossWeight = 0;
      double suppPartWeight = 0;
      double cplArea = 0;
      double cplAreaWHare = 0;

      int totalCplNos = 0;
      double assWeight = 0;
      double totalCplArea = 0;
      double totalCplWeight = 0;
      double totalCplAssWeight = 0;

      string uPhStr = string.Empty;
      string uF4Str = string.Empty;
      string uF3Str = string.Empty;
      string uF2Str = string.Empty;
      string uF1Str = string.Empty;

      List<string> uniqueUph = new List<string>();
      List<string> uniqueUf4 = new List<string>();
      List<string> uniqueUf3 = new List<string>();
      List<string> uniqueUf2 = new List<string>();
      List<string> uniqueUf1 = new List<string>();

      foreach (var cpl in cpls)
      {
        cpl.GetReportProperty("PART_POS", ref partPos);
        cpl.GetReportProperty("ASSEMBLY_POS", ref assPos);
        cpl.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
        cpl.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
        cpl.GetReportProperty("WEIGHT_NET", ref netWeight);
        cpl.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
        cpl.GetReportProperty("ASSEMBLY.WEIGHT_GROSS", ref assWeight);
        cpl.GetReportProperty("ASSEMBLY.SUPPLEMENT_PART_WEIGHT", ref suppPartWeight);
        cpl.GetReportProperty("USER_PHASE", ref userPhase);
        cpl.GetReportProperty("USER_FIELD_4", ref userField4);
        cpl.GetReportProperty("USER_FIELD_3", ref userField3);
        cpl.GetReportProperty("USER_FIELD_2", ref userField2);
        cpl.GetReportProperty("USER_FIELD_1", ref userField1);

        double weightDiff = Math.Round(grossWeight - netWeight, 3);
        double cutOutArea = Math.Round((grossArea - netArea) / 1000000, 3);
        string ps = weightDiff > 0 || suppPartWeight > 0 ? "Shp" : "Pln";
        cplArea = (weightDiff == 0) ? netArea : grossArea;
        cplAreaWHare = (cutOutArea > 0.2) ? netArea : grossArea;

        totalCplNos += 1;
        totalCplArea += cplArea;
        totalCplWeight += grossWeight;
        totalCplAssWeight += assWeight;

        uniqueUph.Add(userPhase);
        uniqueUf4.Add(userField4);
        uniqueUf3.Add(userField3);
        uniqueUf2.Add(userField2);
        uniqueUf1.Add(userField1);
      }
      var totalCplAreaMetric = new Area(totalCplArea).ToMetricUnits();
      var totalCplWeightKgs = new Mass(totalCplWeight).ToMetricUnits();
      var totalCplAssWeightKgs = new Mass(totalCplAssWeight).ToMetricUnits();

      var totalCplAreaSqFt = new Area(totalCplArea).ToImperialUnits();
      var totalCplWeightPounds = new Mass(totalCplWeight).ToImperialUnits();
      var totalCplAssWeightPounds = new Mass(totalCplAssWeight).ToImperialUnits();

      IEnumerable<string> distinctUphs = uniqueUph.Distinct();
      IEnumerable<string> distinctUf4s = uniqueUf4.Distinct();
      IEnumerable<string> distinctUf3s = uniqueUf3.Distinct();
      IEnumerable<string> distinctUf2s = uniqueUf2.Distinct();
      IEnumerable<string> distinctUf1s = uniqueUf1.Distinct();

      string[] distinctUphsArrSorted = distinctUphs.ToArray();
      string[] distinctUf4sArrSorted = distinctUf4s.ToArray();
      string[] distinctUf3sArrSorted = distinctUf3s.ToArray();
      string[] distinctUf2sArrSorted = distinctUf2s.ToArray();
      string[] distinctUf1sArrSorted = distinctUf1s.ToArray();

      Array.Sort(distinctUphsArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf4sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf3sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf2sArrSorted, new AlphanumComparator());
      Array.Sort(distinctUf1sArrSorted, new AlphanumComparator());

      foreach (var item in distinctUphsArrSorted)
      {
        uPhStr += $"{item} ";
      }
      foreach (var item in distinctUf4sArrSorted)
      {
        uF4Str += $"{item} ";
      }
      foreach (var item in distinctUf3sArrSorted)
      {
        uF3Str += $"{item} ";
      }
      foreach (var item in distinctUf2sArrSorted)
      {
        uF2Str += $"{item} ";
      }
      foreach (var item in distinctUf1sArrSorted)
      {
        uF1Str += $"{item} ";
      }
      var uF4Strs = string.Empty;
      var uF3Strs = string.Empty;
      var uF2Strs = string.Empty;
      var uF1Strs = string.Empty;

      if (distinctUf4sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF4Strs = "";
      }
      else
      {
        uF4Strs = $", U/F4s:{uF4Str}";
      }
      if (distinctUf3sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF3Strs = "";
      }
      else
      {
        uF3Strs = $", U/F3s:{uF3Str}";
      }
      if (distinctUf2sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF2Strs = "";
      }
      else
      {
        uF2Strs = $", U/F2s:{uF2Str}";
      }
      if (distinctUf1sArrSorted.Where(x => !string.IsNullOrEmpty(x)).ToArray().Length < 1)
      {
        uF1Strs = "";
      }
      else
      {
        uF1Strs = $", U/F1s:{uF1Str}";
      }

      var userStrs = $"U/Phs:{uPhStr}{uF4Strs}{uF3Strs}{uF2Strs}{uF1Strs}";

      string result = $"Cpl summary:- [Nos:{totalCplNos}]  [Ar:{totalCplAreaMetric} ({totalCplAreaSqFt})]  [Cpl.Wt:{totalCplWeightKgs} ({totalCplWeightPounds})]  [{userStrs}]";
      return result;
    }
  }
}
