namespace Pract5DAA.Algorithm;

public class Algorithm1 : IAlgorithm {
  private string _name = "Voraz";
  public Solution Solve(Instance instance) {
    Solution solution = new Solution();
    PathMap toVisit = new PathMap(instance.Zones.Zones);
    int timeLimit = instance.maximumTime;
    int loadLimit = instance.maximumLoad;
    int num_vehicles = 0;
    List<Truck> vehicles = new List<Truck>();
    PathMap Stations =  new PathMap(instance.Stations);
    while(toVisit.length > 0) {
      Truck currentTruck = new Truck(num_vehicles, loadLimit, timeLimit, instance.speed);
      currentTruck.AddZone(instance.Depot, 0, 0);
      bool flag = true;
      while(flag) {
        if (toVisit.length == 0) {
          break;
        }
        Zone closest = toVisit.ClosestZone(currentTruck.LastZone.Position);
        int timeToNext = currentTruck.LastZone.TimeToNext(closest, instance.speed) + closest.CollectionTime;
        Zone closerStation = Stations.ClosestZone(closest.Position);
        int timeStationFromNext = closest.TimeToNext(closerStation, instance.speed);
        int timeToDepot = closerStation.TimeToNext(instance.Depot, instance.speed);
        int loadNext = currentTruck.CurrentLoad + closest.Load;
        int totallyTime =  timeToNext + timeStationFromNext + timeToDepot;
        if(totallyTime <= timeLimit - currentTruck.CurrentTime && loadNext <= loadLimit ) {
          currentTruck.AddZone(closest, timeToNext, closest.Load);
          toVisit.RemoveZone(closest);
        } else {
          if (totallyTime <= timeLimit - currentTruck.CurrentTime) {
            closerStation = Stations.ClosestZone(currentTruck.LastZone.Position);
            timeToNext = currentTruck.LastZone.TimeToNext(closerStation, instance.speed);
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
      solution.TotalLoad += currentTruck.CurrentLoad;
      solution.TotalTime += currentTruck.CurrentTime;
      num_vehicles++;
    }
    solution.NumVehicles = num_vehicles;
    solution.Trucks = vehicles;
    return solution;
  }
  public string GetName => _name;
}