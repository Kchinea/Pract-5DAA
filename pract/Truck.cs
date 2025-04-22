namespace Pract5DAA;

public class Truck {
  private int _id;
  private int _maximumLoad;
  private int _currentLoad;
  private int _maximumTime;
  private int _currentTime;
  private int _speed;
  private List<Zone> _path;
  private List<int> _pickupTimes;  // Lista para almacenar los tiempos de recogida en cada zona visitada

  public Truck(int id, int maximumLoad, int maximumTime, int speed) {
    _id = id;
    _maximumLoad = maximumLoad;
    _currentLoad = 0;
    _maximumTime = maximumTime;
    _currentTime = 0;
    _speed = speed;
    _path = new List<Zone>();
    _pickupTimes = new List<int>();  // Inicializamos la lista de tiempos de recogida
  }

  // Método para añadir una zona al camión y registrar el tiempo de recogida en esa zona
  public void AddZone(Zone zone, int time, int load) {
    _path.Add(zone);  // Añadimos la zona al camino del camión
    _currentTime += time;  // Actualizamos el tiempo total del camión
    _currentLoad += load;  // Actualizamos la carga del camión

    // Registramos el tiempo de visita para cada zona
    _pickupTimes.Add(_currentTime);  // Registramos el tiempo de la zona actual visitada
  }

  // Clonación del camión (seclonará el tiempo y las zonas)
  public Truck Clone() {
    Truck clone = new Truck(_id, _maximumLoad, _maximumTime, _speed);
    clone._currentLoad = _currentLoad;
    clone._currentTime = _currentTime;
    clone._path = new List<Zone>(_path.Select(z => z.Clone()).ToList());
    return clone;
  }


  // Representación del camión
  public override string ToString() {
    return $"Truck {_id}:\n" +
           $"Maximum Load: {_maximumLoad}, Current Load: {_currentLoad}, " +
           $"Maximum Time: {_maximumTime}, Current Time: {_currentTime}, " +
           $"Speed: {_speed}, Path: {_path}";
  }

  // Acceso a la última zona del camión
  public Zone LastZone {
    get => _path.Last();
  }

  // Acceso a la lista de zonas que recorre el camión
  public List<Zone> Path {
    get => _path;
  }

  // Propiedades para acceder a los datos del camión
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
  public int MaximumLoad {
    get => _maximumLoad;
  }
  public int MaximumTime {
    get => _maximumTime;
  }
  public int Speed {
    get => _speed;
  }

  // Métodos para obtener los tiempos de recogida
  public List<int> PickupTimes {
    get => _pickupTimes;  // Devuelve la lista de todos los tiempos de recogida
  }

  // Método para obtener el tiempo de recogida en una zona específica (si la zona está registrada)
  public int GetPickupTime(Zone zone) {
    int index = _path.IndexOf(zone);
    if (index != -1 && index < _pickupTimes.Count) {
      return _pickupTimes[index];
    }
    return -1;  // Si no hay tiempo registrado para esa zona, devolvemos -1
  }
}
