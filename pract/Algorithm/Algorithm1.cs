namespace Pract5DAA.Algorithm;

public class Algorithm1 : IAlgorithm {
  public void Solve(Instance instance) {
    Console.WriteLine("Solving instance by 1");
    PathMap toVisit = instance.Zones;
    List<Zone> solution = new List<Zone>();
    int timeLimit = instance.maximumTime;
    int loadLimit = instance.maximumLoad;
    int num_vehicles = 1;
    List<Truck> vehicles = new List<Truck>();
    PathMap Stations =  new PathMap(instance.Stations);
    while(true) {
      Truck currentTruck = new Truck(num_vehicles, loadLimit, timeLimit, instance.speed);
      currentTruck.AddZone(Stations.Zones[0], 0, 0);
      vehicles.Add(currentTruck);
      num_vehicles++;
      //todo lo de abajo se podria hacer en una funcion
      Zone closest = instance.Zones.ClosestZone(Stations.Zones[0].Position); //esto deberia ser la ultima del path, debo poner el depot arriba y que asi lo siga 
      int timeToNext = currentTruck.LastZone.TimeToNext(closest, instance.speed);
      Zone closerStation = Stations.ClosestZone(closest.Position); 
      int timeStationFromNext = closest.TimeToNext(closerStation, instance.speed);
      int timeToStation = closerStation.TimeToNext(instance.Depot, instance.speed);
      double loadToNext = currentTruck.CurrentLoad + closest.Load;
      Console.WriteLine($"Closest zone: {closest}");
      break;
    }
  }
}