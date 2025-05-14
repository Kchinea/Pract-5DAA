namespace Pract5DAA;

public class Solution {
  private List<Truck> _trucks;
  private double _totalTime;
  private double _totalLoad;
  private int _numVehicles;
  public Solution()
  {
    _trucks = new List<Truck>();
    _totalTime = 0;
    _totalLoad = 0;
    _numVehicles = 0;
  }

  public Solution(List<Truck> trucks)
  {
    _trucks = trucks;
    _totalTime = 0;
    _totalLoad = 0;
    foreach (Truck truck in _trucks)
    {
      _totalTime += truck.CurrentTime;
      _totalLoad += truck.CurrentLoad;
    }
    _numVehicles = _trucks.Count;
  }
  public List<Truck> Trucks {
    get => _trucks;
    set => _trucks = value;
  }
  public double TotalTime {
    get => _totalTime;
    set => _totalTime = value;
  }
  public double TotalLoad {
    get => _totalLoad;
    set => _totalLoad = value;
  }
  public int NumVehicles {
    get => _numVehicles;
    set => _numVehicles = value;
  }
  public override string ToString()
  {
    var result = new System.Text.StringBuilder();
    for (int i = 0; i < _trucks.Count; i++)
    {
      result.AppendLine($"Truck {i + 1}:");
      result.AppendLine($"  Zones: {string.Join(", ", _trucks[i].Path)}");
    }
    return result.ToString();
  }
}

// namespace Pract5DAA;

// public class Solution {
//   public int NumVehicles { get; set; }
//   public int TotalTime { get; set; }
//   public int TotalLoad { get; set; }
//   public List<Truck> Trucks { get; set; }

//   public Solution() {
//     NumVehicles = 0;
//     TotalTime = 0;
//     TotalLoad = 0;
//     Trucks = new List<Truck>();
//   }
// }