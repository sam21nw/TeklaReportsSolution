using AppExtensions;

using System;
using System.Collections;
using System.Collections.Generic;

using Tekla.Structures.Model;

namespace TeklaDisplayInfo
{
  internal class ModelObjectsSchedule
  {
    public static ModelObjectProperties GetObjectReportProperties(ModelObject mo)
    {
      string name = string.Empty;
      string grAssPos = string.Empty;
      int modelTotal = 0;
      double grLength = 0.000;
      double grWidth = 0.000;
      double grossArea = 0.000;
      double netArea = 0.000;
      double netWeight = 0.000;
      double grossWeight = 0.000;
      double topLevel = 0.000;
      double bottomLevel = 0.000;
      string userPhase = string.Empty;
      string userField4 = string.Empty;
      string userField3 = string.Empty;
      string position = string.Empty;

      ArrayList secondaries = ((Part)mo).GetAssembly().GetSecondaries();
      ModelObjectProperties moProps = new ModelObjectProperties();

      mo.GetReportProperty("NAME", ref name);
      mo.GetReportProperty("ASSEMBLY_POS", ref grAssPos);
      mo.GetReportProperty("MODEL_TOTAL", ref modelTotal);
      mo.GetReportProperty("LENGTH", ref grLength);
      mo.GetReportProperty("HEIGHT", ref grWidth);
      mo.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref grossArea);
      mo.GetReportProperty("AREA_PROJECTION_XY_NET", ref netArea);
      mo.GetReportProperty("USER_PHASE", ref userPhase);
      mo.GetReportProperty("USER_FIELD_4", ref userField4);
      mo.GetReportProperty("USER_FIELD_3", ref userField3);
      mo.GetReportProperty("WEIGHT_NET", ref netWeight);
      mo.GetReportProperty("WEIGHT_GROSS", ref grossWeight);
      mo.GetReportProperty("TOP_LEVEL_UNFORMATTED", ref topLevel);
      mo.GetReportProperty("BOTTOM_LEVEL_UNFORMATTED", ref bottomLevel);
      mo.GetReportProperty("ASSEMBLY.ASSEMBLY_POSITION_CODE", ref position);

      moProps.Name = name;
      moProps.TopLevel = topLevel.MMtoFeetInches();
      moProps.BottomLevel = bottomLevel.MMtoFeetInches();
      moProps.PartPosition = ((Part)mo).GetPartMark();
      moProps.AssemblyPosition = grAssPos;
      moProps.Quantity = modelTotal;

      moProps.Length = Math.Round(grLength, 0, MidpointRounding.AwayFromZero);
      moProps.Width = Math.Round(grWidth, 0, MidpointRounding.AwayFromZero);
      moProps.LengthImperial = grLength.MMtoFeetInches();
      moProps.WidthImperial = grWidth.MMtoFeetInches();

      double weightDiff = grossWeight - netWeight;
      double gratingArea = (weightDiff == 0) ? netArea : grossArea;
      //double gratingArea = grossArea;

      double cutArea = Math.Round((grossArea - netArea) * 1E-06, 3);

      double whArea = cutArea > 0.2 ? netArea : grossArea;

      moProps.CutArea = cutArea;
      moProps.Area = gratingArea * 1E-06;
      moProps.WHArea = whArea * 1E-06;
      moProps.UserPhase = userPhase;
      moProps.UserField4 = userField4;
      moProps.UserField3 = userField3;
      moProps.Position = position;

      if (secondaries != null)
      {
        var tpLength = 0.000;
        var bbLength = 0.000;
        var nsLength = 0.000;
        var chqTotalArea = 0.000;
        foreach (ModelObject secondary in secondaries)
        {
          var prefix = string.Empty;
          secondary.GetReportProperty("PREFIX", ref prefix);
          if (secondary is Part tp && (prefix == "TP" || tp.Name.ToUpper().Contains("TOE")))
          {
            string assPos = string.Empty;
            double length = 0.000;
            tp.GetReportProperty("ASSEMBLY_POS", ref assPos);
            tp.GetReportProperty("LENGTH", ref length);

            tpLength += Math.Round(length, 0);
          }
          if (secondary is Part bb && (prefix == "BB" || bb.Name.ToUpper().Contains("BIND") || bb.Name.ToUpper().Contains("BAND")))
          {
            string assPos = string.Empty;
            double length = 0.000;
            bb.GetReportProperty("ASSEMBLY_POS", ref assPos);
            bb.GetReportProperty("LENGTH", ref length);

            bbLength += Math.Round(length, 0);
          }
          if (secondary is Part ns && (ns.Name.ToUpper().Contains("NOSING_PLATE")))
          {
            string assPos = string.Empty;
            double length = 0.000;
            double height = 0.000;
            double ln = 0.000;
            ns.GetReportProperty("ASSEMBLY_POS", ref assPos);
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

            nsLength += Math.Round(ln, 0);
          }
          if (secondary is Part chq && (chq.Name.ToUpper().Contains("CHEQ") || prefix == "CPL" || prefix == "CHQ"))
          {
            string assPos = string.Empty;
            double chqArea = 0.000;
            chq.GetReportProperty("ASSEMBLY_POS", ref assPos);
            chq.GetReportProperty("AREA_PROJECTION_XY_GROSS", ref chqArea);

            chqTotalArea += Math.Round(chqArea * 1E-06, 3);
          }
          moProps.TpLength = tpLength;
          moProps.BbLength = bbLength;
          moProps.NsLength = nsLength;
          moProps.ChqArea = chqTotalArea;
        }
      }

      return moProps;
    }
  }
}
