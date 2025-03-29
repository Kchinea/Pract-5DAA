namespace Pract5DAA;

public class Instance {
  private int _maxCollectionDuration;
  private int _maxDeliveryDuration;
  private int _numVehicles;
  private int _numZones;
  private int _Lx;
  private int _Ly;
  private int _maxCollectionCapacity;
  private int _maxDeliveryCapacity;
  private int _speed;
  private Point _depotPosition;
  private Point _firstStationPosition;
  private Point _lastStationPosition;
  private Point _dumpPosition; 
  private int _epsilon;
  private int _offset;
  private int _k;
  private PathMap _zones;


  public Instance(int maxCollectionDuration, int maxDeliveryDuration, int numVehicles, int numZones, int Lx, int Ly, int maxCollectionCapacity, int maxDeliveryCapacity, int speed, Point depotPosition, Point firstStationPosition, Point lastStationPosition, Point dumpPosition, int epsilon, int offset, int k, PathMap zones) {
    this._maxCollectionDuration = maxCollectionDuration;
    this._maxDeliveryDuration = maxDeliveryDuration;
    this._numVehicles = numVehicles;
    this._numZones = numZones;
    this._Lx = Lx;
    this._Ly = Ly;
    this._maxCollectionCapacity = maxCollectionCapacity;
    this._maxDeliveryCapacity = maxDeliveryCapacity;
    this._speed = speed;
    this._depotPosition = depotPosition;
    this._firstStationPosition = firstStationPosition;
    this._lastStationPosition = lastStationPosition;
    this._dumpPosition = dumpPosition;
    this._epsilon = epsilon;
    this._offset = offset;
    this._k = k;
    this._zones = zones;
  }
  public override string ToString() {
    return $"Instance:\n" +
           $"  Max Collection Duration: {_maxCollectionDuration}\n" +
           $"  Max Delivery Duration: {_maxDeliveryDuration}\n" +
           $"  Num Vehicles: {_numVehicles}\n" +
           $"  Num Zones: {_numZones}\n" +
           $"  Lx: {_Lx}, Ly: {_Ly}\n" +
           $"  Max Collection Capacity: {_maxCollectionCapacity}\n" +
           $"  Max Delivery Capacity: {_maxDeliveryCapacity}\n" +
           $"  Speed: {_speed}\n" +
           $"  Depot Position: {_depotPosition}\n" +
           $"  First Station Position: {_firstStationPosition}\n" +
           $"  Last Station Position: {_lastStationPosition}\n" +
           $"  Dump Position: {_dumpPosition}\n" +
           $"  Epsilon: {_epsilon}, Offset: {_offset}, k: {_k}\n" +
           $"  Zones:\n    {string.Join("\n    ", _zones)}";
  }

  public int MaximumTrucks {
    get => _numVehicles;
  }
  public int maximumTime {
    get => _maxCollectionDuration;
  }
  public int maximumLoad {
    get => _maxCollectionCapacity;
  }
  public int speed {
    get => _speed;
  }
  public PathMap Zones {
    get => _zones;
  }
  public Point DepotPosition {
    get => _depotPosition;
  }
  public Point FirstStationPosition {
    get => _firstStationPosition;
  }
  public Point LastStationPosition {
    get => _lastStationPosition;
  }
  public Point DumpPosition {
    get => _dumpPosition;
  }
}