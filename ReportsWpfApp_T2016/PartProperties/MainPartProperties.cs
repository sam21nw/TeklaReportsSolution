namespace PartProperties
{
  public class MainPartProperties
  {
    public string Name;

    public string AssemblyPos { get; internal set; }

    public string PartPos { get; internal set; }

    public int Quantity { get; internal set; }

    public string Length_Imperial { get; internal set; }

    public string Width_Imperial { get; internal set; }

    public double Length { get; internal set; }

    public double Width { get; internal set; }

    public double Area { get; internal set; }

    public double Weight { get; internal set; }

    public double CutArea { get; internal set; }

    public double TP_Length { get; internal set; }

    public double NS_Length { get; internal set; }

    public double BB_Length { get; internal set; }

    public double CHQ_Area { get; internal set; }

    public string Top_Level { get; internal set; }

    public string Position { get; internal set; }

    public string UserPhase { get; internal set; }

    public string UserField4 { get; internal set; }

    public string UserField3 { get; internal set; }

    public string UserField2 { get; internal set; }

    public string UserField1 { get; internal set; }

    public int ID { get; internal set; }
  }
}
