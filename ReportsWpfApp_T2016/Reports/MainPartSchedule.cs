
using System;
using System.Collections;
using System.Collections.Generic;

using Tekla.Structures.Model;

using AppExtensions;
using PartProperties;

namespace Reports
{
  public static class MainPartSchedule
  {
    private static double wHAreaCut;
    private static double unitWeight;

    public static double WHAreaCut { get => wHAreaCut; set => wHAreaCut = value; }
    public static double UnitWeight { get => unitWeight; set => unitWeight = value; }
    public static void GetMainPartReportProperties(Part PrimaryPart, List<MainPartProperties> MainPartList)
    {
      string name = string.Empty;
      string mpName = string.Empty;
      string assPos = string.Empty;
      int modelTotal = 0;
      double grLength = 0.000;
      double grWidth = 0.000;
      double grossArea = 0.000;
      double netArea = 0.000;
      double netWeight = 0.0000;
      double grossWeight = 0.000;
      double Top_Level = 0.000;
      string userPhase = string.Empty;
      string userField4 = string.Empty;
      string userField3 = string.Empty;
      string userField2 = string.Empty;
      string userField1 = string.Empty;
      string position = string.Empty;
      var TP_Length = 0.000;
      var BB_Length = 0.000;
      var NS_Length = 0.000;
      var chqTotalArea = 0.000;

      if (PrimaryPart == null)
      {
        return;
      }
      Part MainPart = (Part)PrimaryPart.GetAssembly().GetMainPart();
      ArrayList secondaries = PrimaryPart.GetAssembly().GetSecondaries();
      MainPartProperties MainPartProperties = new MainPartProperties();

      MainPart.GetReportProperty("NAME", ref mpName);
      PrimaryPart.GetReportProperty("NAME", ref name);
      PrimaryPart.GetReportProperty("ASSEMBLY_POS", ref assPos);
      PrimaryPart.GetReportProperty("MODEL_TOTAL", ref modelTotal);
      PrimaryPart.GetReportProperty("LENGTH", ref grLength);
      PrimaryPart.GetReportProperty("HEIGHT", ref grWidth);
      PrimaryPart.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
      PrimaryPart.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
      PrimaryPart.GetReportProperty("USER_PHASE", ref userPhase);
      PrimaryPart.GetReportProperty("USER_FIELD_4", ref userField4);
      PrimaryPart.GetReportProperty("USER_FIELD_3", ref userField3);
      PrimaryPart.GetReportProperty("USER_FIELD_2", ref userField2);
      PrimaryPart.GetReportProperty("USER_FIELD_1", ref userField1);
      PrimaryPart.GetReportProperty("WEIGHT_NET", ref netWeight);
      PrimaryPart.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
      PrimaryPart.GetReportProperty("TOP_LEVEL_UNFORMATTED", ref Top_Level);
      PrimaryPart.GetReportProperty("ASSEMBLY.ASSEMBLY_POSITION_CODE", ref position);

      MainPartProperties.ID = PrimaryPart.Identifier.ID;
      MainPartProperties.Name = name;
      MainPartProperties.AssemblyPos = assPos;
      MainPartProperties.PartPos = PrimaryPart.GetPartMark();
      var mpm = MainPart.GetPartMark();

      //MainPartProperties.Quantity = modelTotal;
      MainPartProperties.Length = Math.Round(grLength, 0, MidpointRounding.AwayFromZero);
      MainPartProperties.Width = Math.Round(grWidth, 0, MidpointRounding.AwayFromZero);
      MainPartProperties.Length_Imperial = grLength.MMtoFeetInches();
      MainPartProperties.Width_Imperial = grWidth.MMtoFeetInches();
      MainPartProperties.UserPhase = userPhase;
      MainPartProperties.UserField4 = userField4;
      MainPartProperties.UserField3 = userField3;
      MainPartProperties.UserField2 = userField2;
      MainPartProperties.UserField1 = userField1;
      MainPartProperties.Top_Level = Top_Level.MMtoFeetInches();
      MainPartProperties.Position = position;

      MainPartProperties.Weight = Math.Round(grossWeight, 3, MidpointRounding.AwayFromZero);

      if (MainPartProperties.PartPos == mpm && name.ToUpper().Contains("GR") || name.ToUpper().Contains("CH") || name.ToUpper().Contains("CP") || name.ToUpper().Contains("PL"))
      {
        //double weightDiff = grossWeight - netWeight;
        double grArea = Math.Round(MainPartProperties.Length * MainPartProperties.Width * 1E-06, 3, MidpointRounding.AwayFromZero);

        double cutArea = Math.Round((grossArea - netArea) * 1E-06, 3);
        double gratingArea;
        gratingArea = cutArea > wHAreaCut ? grArea - cutArea : grArea;

        MainPartProperties.CutArea = cutArea;
        MainPartProperties.Area = gratingArea;

        if (1 <= unitWeight && unitWeight <= 500)
        {
          MainPartProperties.Weight = Math.Round(gratingArea * unitWeight, 3, MidpointRounding.AwayFromZero);
        }

        if (secondaries != null)
        {
          foreach (Part secondary in secondaries)
          {
            var prefix = string.Empty;
            secondary.GetReportProperty("PREFIX", ref prefix);
            if (secondary is Part tp && (prefix == "TP" || tp.Name.ToUpper().Contains("TOE")))
            {
              double length = 0.000;
              tp.GetReportProperty("LENGTH", ref length);

              TP_Length += Math.Round(length, 0);
            }
            if (secondary is Part bb && (prefix == "BB" || bb.Name.ToUpper().Contains("BIND") || bb.Name.ToUpper().Contains("BAND")))
            {
              double length = 0.000;
              bb.GetReportProperty("LENGTH", ref length);

              BB_Length += Math.Round(length, 0);
            }
            if (secondary is Part ns && ns.Name.ToUpper().Contains("NOSING_PLATE"))
            {
              double length = 0.000;
              double height = 0.000;
              double ln = 0.000;
              ns.GetReportProperty("LENGTH", ref length);
              ns.GetReportProperty("HEIGHT", ref height);

              if (ns is PolyBeam || height > length)
              {
                ln = height;
              }
              else
              {
                ln = length;
              }

              NS_Length += Math.Round(ln, 0);
            }
            if (secondary is Part chq && (chq.Name.ToUpper().Contains("CHEQ") || prefix == "CPL" || prefix == "CHQ"))
            {
              double CHQ_Length = 0.000;
              chq.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref CHQ_Length);

              chqTotalArea += Math.Round(CHQ_Length * 1E-06, 3);
            }
            MainPartProperties.TP_Length = TP_Length;
            MainPartProperties.BB_Length = BB_Length;
            MainPartProperties.NS_Length = NS_Length;
            MainPartProperties.CHQ_Area = chqTotalArea;
          }
        }
      }
      MainPartList.Add(MainPartProperties);
    }
  }
}
