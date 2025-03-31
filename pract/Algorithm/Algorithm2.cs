namespace Pract5DAA.Algorithm;

public class Algorithm2 : IAlgorithm {
  private string _name = "GRASP";

  public int Solve(Instance instance) {
    Console.WriteLine("Solving instance by 2");
    PathMap toVisit = new PathMap(instance.Zones.Zones);
    int timeLimit = instance.maximumTime;
    int loadLimit = instance.maximumLoad;
    int num_vehicles = 1;
    Console.WriteLine($"{instance.Depot.Position}");
    List<Truck> vehicles = new List<Truck>();
    PathMap Stations =  new PathMap(instance.Stations);
    while(toVisit.length > 0) {
      Truck currentTruck = new Truck(num_vehicles, loadLimit, timeLimit, instance.speed);
      currentTruck.AddZone(instance.Depot, 0, 0);
      bool flag = true;
      while(flag) {
        Console.WriteLine($"Truck {currentTruck.Id} is in zone {currentTruck.LastZone.Id}");
        if (toVisit.length == 0) {
          Console.WriteLine("Numero de camiones necesarios: " + num_vehicles);
          break;
        }
        Zone closest = toVisit.RandomClosestZone(currentTruck.LastZone.Position);
        int timeToNext = currentTruck.LastZone.TimeToNext(closest, instance.speed);
        Zone closerStation = Stations.ClosestZone(closest.Position);
        int timeStationFromNext = closest.TimeToNext(closerStation, instance.speed);
        int timeToDepot = closerStation.TimeToNext(instance.Depot, instance.speed);
        int loadNext = currentTruck.CurrentLoad + closest.Load;
        int totallyTime =  timeToNext + timeStationFromNext + timeToDepot;
        // Console.WriteLine($"ActuallyTime: {currentTruck.CurrentTime}Time to next {timeToNext}, Time to station {timeStationFromNext}, Time to depot {timeToDepot}, Load {loadNext}, Time {totallyTime}");
        if(totallyTime <= timeLimit - currentTruck.CurrentTime && loadNext <= loadLimit ) {
          currentTruck.AddZone(closest, timeToNext, closest.Load);
          toVisit.RemoveZone(closest);
        } else {
          if (totallyTime <= timeLimit - currentTruck.CurrentTime) {
            closerStation = Stations.ClosestZone(currentTruck.LastZone.Position);
            timeToNext = currentTruck.LastZone.TimeToNext(closerStation, instance.speed);
            // Console.WriteLine($"Time to next {timeToNext}");
            currentTruck.AddZone(closerStation, timeToNext, closest.Load);
            currentTruck.CurrentLoad = 0;
          } else {
            break;
          }
        }
      }
      if (!instance.Stations.Contains(currentTruck.LastZone)) {
        currentTruck.AddZone(Stations.ClosestZone(currentTruck.LastZone.Position), currentTruck.LastZone.TimeToNext(Stations.ClosestZone(currentTruck.LastZone.Position), instance.speed), 0);
        currentTruck.AddZone(instance.Depot, currentTruck.LastZone.TimeToNext(instance.Depot, instance.speed), 0);
      } else {
        currentTruck.AddZone(instance.Depot, currentTruck.LastZone.TimeToNext(instance.Depot, instance.speed), 0);
      }
      vehicles.Add(currentTruck);
      num_vehicles++;
    }
    return num_vehicles;
  }
  public string GetName => _name;
}