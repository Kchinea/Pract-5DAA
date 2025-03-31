namespace Pract5DAA;

public class Truck {
  private int _id;
  private int _maximumLoad;
  private int _currentLoad;
  private int _maximumTime;
  private int _currentTime;
  private int _speed;
  private List<Zone> _path;
  public Truck( int id, int maximumLoad, int maximumTime, int speed) {
    _id = id;
    _maximumLoad = maximumLoad;
    _currentLoad = 0;
    _maximumTime = maximumTime;
    _currentTime = 0;
    _speed = speed;
    _path = new List<Zone>();
  }
  public void AddZone(Zone zone, int time, int load) {
    _path.Add(zone);
    _currentTime += time;
    _currentLoad += load;
  }
  public bool CanAddZone(Zone zone, int time, int load) {
    return _currentLoad + load <= _maximumLoad && _currentTime + time <= _maximumTime;
  }
    
  public override string ToString() {
    return $"Truck {_id}:\n" + 
           $"Maximum Load: {_maximumLoad}, Current Load: {_currentLoad}, " +
           $"Maximum Time: {_maximumTime}, Current Time: {_currentTime}, " +
           $"Speed: {_speed}, Path: {_path}";
    }

  public Zone LastZone {
    get => _path.Last();
  }
  public List<Zone> Path {
    get => _path;
  }
  public int Id {
    get => _id;
  }
  public int CurrentLoad {
    get => _currentLoad;
    set => _currentLoad = value;
  } 
  public int CurrentTime {
    get => _currentTime;
  }
}