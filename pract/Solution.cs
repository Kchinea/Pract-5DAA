namespace Pract5DAA;

public class Solution {
  public int NumVehicles { get; set; }
  public int TotalLoad { get; set; }
  public List<Truck> Trucks { get; set; }

  public Solution() {
    NumVehicles = 0;
    TotalLoad = 0;
    Trucks = new List<Truck>();
  }

  public Solution Clone() {
    return new Solution {
      NumVehicles = this.NumVehicles,
      TotalLoad = this.TotalLoad,
      Trucks = this.Trucks.Select(t => t.Clone()).ToList()
    };
  }

  public int TotalTime {
    get => Trucks.Sum(t => t.CurrentTime);
  }
public override string ToString() {
  return $"Solution:\n" +
         $"- Vehicles: {NumVehicles}\n" +
         $"- Total Load: {TotalLoad}\n" +
         $"- Total Time: {TotalTime}\n" +
         $"- Trucks:\n" +
         string.Join("\n", Trucks.Select(t => "  " + t.ToString()));
}

}
