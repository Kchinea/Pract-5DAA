namespace Pract5DAA;

public class Solution {
  public int NumVehicles { get; set; }
  public int TotalTime { get; set; }
  public int TotalLoad { get; set; }
  public List<Truck> Trucks { get; set; }

  public Solution() {
    NumVehicles = 0;
    TotalTime = 0;
    TotalLoad = 0;
    Trucks = new List<Truck>();
  }
}