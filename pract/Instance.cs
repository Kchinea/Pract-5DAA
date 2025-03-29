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
  private Zone _depot;
  private List<Zone> _stations;
  private Point _dumpPosition; 
  private int _epsilon;
  private int _offset;
  private int _k;
  private PathMap _zones;


  public Instance(int maxCollectionDuration, int maxDeliveryDuration, int numVehicles, int numZones, int Lx, int Ly, int maxCollectionCapacity, int maxDeliveryCapacity, int speed, Zone depot, List<Zone> stations, Point dumpPosition, int epsilon, int offset, int k, PathMap zones) {
    this._maxCollectionDuration = maxCollectionDuration;
    this._maxDeliveryDuration = maxDeliveryDuration;
    this._numVehicles = numVehicles;
    this._numZones = numZones;
    this._Lx = Lx;
    this._Ly = Ly;
    this._maxCollectionCapacity = maxCollectionCapacity;
    this._maxDeliveryCapacity = maxDeliveryCapacity;
    this._speed = speed;
    this._depot = depot;
    this._stations = stations;
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
           $"  Depot Position: {_depot}\n" + //hay que repasarlo, faltan las stations
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
  public Zone Depot {
    get => _depot;
  }
  public Point FirstStationPosition {
    get => _stations[0].Position;
  }
  public Point LastStationPosition {
    get => _stations[1].Position;
  }
  public List<Zone> Stations {
    get => _stations;
  }
  public Point DumpPosition {
    get => _dumpPosition;
  }
}