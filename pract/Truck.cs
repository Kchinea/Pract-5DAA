namespace Pract5DAA;

public class Truck {
  private int _id;
  private int _maximumLoad;
  private int _currentLoad;
  private int _maximumTime;
  private int _currentTime;
  private int _speed;
  private List<(Zone zone, int load,int time)> _path;
  private PathMap _map;
  public Truck( int id, int maximumLoad, int maximumTime, int speed, PathMap map) {
    _id = id;
    _maximumLoad = maximumLoad;
    _currentLoad = 0;
    _maximumTime = maximumTime;
    _currentTime = 0;
    _speed = speed;
    _path = new List<(Zone , int,int)>();
    _map = map;
  }
  public void AddZone(Zone zone, int time, int load) {
    _path.Add((zone, _currentTime += time, _currentLoad += load));
    // _currentTime += time;
    // _currentLoad += load;
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
    get => _path.Last().zone;
  }
  public List<Zone> Path {
    get => _path.Select(entry => entry.zone).ToList();
  }
  public List<(Zone, int,int)> SetPath {
    get => _path;
    set => _path = value;
  }
  public int Id {
    get => _id;
    set => _id = value;
  }
  public int CurrentLoad {
    get => _currentLoad;
    set => _currentLoad = value;
  } 
  public int CurrentTime {
    get => _currentTime;
    set => _currentTime = value;
  }
  public int MaximumLoad {
    get => _maximumLoad;
    set => _maximumLoad = value;
  }
  public int MaximumTime {
    get => _maximumTime;
    set => _maximumTime = value;
  }
  public int Speed {
    get => _speed;
    set => _speed = value;
  }
  public PathMap Map {
    get => _map;
    set => _map = value;
  }

  public Truck Clone()
  {
      var clonedTruck = new Truck(_id, _maximumLoad, _maximumTime, _speed, _map);
      clonedTruck._currentLoad = _currentLoad;
      clonedTruck._currentTime = _currentTime;
      // Clonar la ruta (shallow copy de Zone, deep copy de la lista)
      clonedTruck._path = new List<(Zone zone, int load, int time)>(_path);
      return clonedTruck;
  }
public void RecalculatePath()
{
    _currentTime = 0;
    _currentLoad = 0;
    var newPath = new List<(Zone zone, int load, int time)>();

    for (int i = 0; i < _path.Count; i++)
    {
        var zone = _path[i].zone;
        int load = zone.Load;
        int time = 0;
        if (i > 0)
        {
            var prevZone = _path[i - 1].zone;
            time = (int)(prevZone.Position.CalculateDistance(zone.Position) / _speed);
        }
        _currentTime += time;
        _currentLoad += load;
        newPath.Add((zone, _currentLoad, _currentTime));
    }
    SetPath = newPath;
}
}