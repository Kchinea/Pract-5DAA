namespace Pract5DAA;

public class Reader {
  private string _directory;
  public Reader(string directory) {
    _directory = directory;
  }

  public List<Instance> ReadAll() {
    List<Instance> instances = new List<Instance>();
    try {
      string[] files = Directory.GetFiles(_directory, "*.txt").OrderBy(f => f.Trim( new Char[] { 'i', '*', '.' } )).ToArray();
      foreach (string file in files) {
        Reader reader = new Reader(file);
        instances.Add(reader.Read());
      }
    }
    catch (Exception e) {
      Console.WriteLine($"Error al leer archivos en {_directory}: {e.Message}");
    }
    return instances;
  }
  public Instance Read() {

    int maxCollectionDuration = 0, maxDeliveryDuration = 0;
    int numVehicles = 0, numZones = 0;
    int Lx = 0, Ly = 0;
    int maxCollectionCapacity = 0, maxDeliveryCapacity = 0;
    int speed = 0;
    Zone  depot = new Zone(0,new Point(0, 0),0,0);
    Point dumpPosition = new Point(0, 0);
    List<Zone> stations = new List<Zone>();
    int epsilon = 0, offset = 0, k = 0;
    List<Zone> zones = new List<Zone>();


    String? line;
    try {
      StreamReader sr = new StreamReader(this._directory);
      line = sr.ReadLine();
      while (line != null) {
        string[] words = line.Split(' ');
        switch (words[0]) {
          case "L1":
              maxCollectionDuration = int.Parse(words[1]);
              break;
          case "L2":
              maxDeliveryDuration = int.Parse(words[1]);
              break;
          case "num_vehicles":
              numVehicles = int.Parse(words[1]);
              break;
          case "num_zones":
              numZones = int.Parse(words[1]);
              break;
          case "Lx":
              Lx = int.Parse(words[1]);
              break;
          case "Ly":
              Ly = int.Parse(words[1]);
              break;
          case "Q1":
              maxCollectionCapacity = int.Parse(words[1]);
              break;
          case "Q2":
              maxDeliveryCapacity = int.Parse(words[1]);
              break;
          case "V":
              speed = int.Parse(words[1]);
              break;
          case "Depot":
              Point depotPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              depot = new Zone(0, depotPosition, 0, 0);
              break;
          case "IF":
              Point firstStationPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              stations.Add( new Zone (-1 ,firstStationPosition, 0, 0));
              break;
          case "IF1":
              Point lastStationPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              stations.Add( new Zone (-2 ,lastStationPosition, 0, 0));
              break;
          case "Dumpsite":
              dumpPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              break;
          case "epsilon":
              epsilon = int.Parse(words[1]);
              break;
          case "offset":
              offset = int.Parse(words[1]);
              break;
          case "k":
              k = int.Parse(words[1]);
              break;
          default:
                int id = int.Parse(words[0]);
                int x = int.Parse(words[1]);  
                int y = int.Parse(words[2]);
                Point cord = new Point(x, y);
                int d1 = int.Parse(words[3]);
                int d2 = int.Parse(words[4]);
                zones.Add(new Zone(id,cord, d1, d2));
              break;
        }
        line = sr.ReadLine();
      }
      //close the file
      sr.Close();
    }
    catch(Exception e) {
      Console.WriteLine(e.Message);
    }
      PathMap pathMap = new PathMap(zones);
      Instance instance = new Instance(_directory,
        maxCollectionDuration, maxDeliveryDuration, numVehicles, numZones,
        Lx, Ly, maxCollectionCapacity, maxDeliveryCapacity, speed,
        depot, stations, dumpPosition,
        epsilon, offset, k, pathMap
        );
        return instance;

  }
}