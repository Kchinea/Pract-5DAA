namespace Pract5DAA;

public class Reader {
  private string _file;
  public Reader(string file) {
    _file = file;
  }
  public Instance Read() {

    int maxCollectionDuration = 0, maxDeliveryDuration = 0;
    int numVehicles = 0, numZones = 0;
    int Lx = 0, Ly = 0;
    int maxCollectionCapacity = 0, maxDeliveryCapacity = 0;
    int speed = 0;
    Point depotPosition = new Point(0, 0);
    Point firstStationPosition = new Point(0, 0);
    Point lastStationPosition = new Point(0, 0);
    Point dumpPosition = new Point(0, 0);
    int epsilon = 0, offset = 0, k = 0;
    List<Zone> zones = new List<Zone>();


    String? line;
    try {
      StreamReader sr = new StreamReader(this._file);
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
              depotPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              break;
          case "IF":
              firstStationPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
              break;
          case "IF1":
              lastStationPosition = new Point(int.Parse(words[1]), int.Parse(words[2]));
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
                int x = (int)double.Parse(words[1]);  
                int y = (int)double.Parse(words[2]);
                Point cord = new Point(x, y);
                double d1 = double.Parse(words[3]);
                double d2 = double.Parse(words[4]);
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
      Instance instance = new Instance(
        maxCollectionDuration, maxDeliveryDuration, numVehicles, numZones,
        Lx, Ly, maxCollectionCapacity, maxDeliveryCapacity, speed,
        depotPosition, firstStationPosition, lastStationPosition, dumpPosition,
        epsilon, offset, k, pathMap
        );

        Console.Write(instance);
        return instance;

  }
}