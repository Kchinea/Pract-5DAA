using Pract5DAA;

public class AlgoritmoFinal {
  public AlgoritmoFinal() {}

  public Solution Solve(Solution initialSolution, Instance instance) {
    Solution currentSolution = initialSolution.Clone();

    List<Truck> transportTrucks = new List<Truck>();

    foreach (Truck cv in currentSolution.Trucks) {
      for (int i = 0; i < cv.Path.Count; i++) {
        Zone zone = cv.Path[i];
        if (zone.Id == -1 || zone.Id == -2) {
          int arrivalTime = cv.PickupTimes[i];

          Truck? bestTruck = null;
          int bestCost = int.MaxValue;

          foreach (Truck tv in transportTrucks) {
            int travelTime = (int)(tv.LastZone.Position.CalculateDistance(zone.Position) / tv.Speed);
            int arrivalAtZone = tv.CurrentTime + travelTime;

            if (arrivalAtZone <= arrivalTime) {
              int cost = arrivalTime - arrivalAtZone;
              if (cost < bestCost) {
                bestTruck = tv;
                bestCost = cost;
              }
            }
          }

          if (bestTruck == null) {
            bestTruck = new Truck(
              transportTrucks.Count + 1000,
              instance.maxDeliveryCapacity,
              instance.maxDeliveryDuration,
              instance.speed
            );
            Zone dumpStart = new Zone(-3, instance.DumpPosition, 0, 0);
            bestTruck.AddZone(dumpStart, 0, 0);
            transportTrucks.Add(bestTruck);
          }

          int moveTime = (int)bestTruck.LastZone.Position.CalculateDistance(zone.Position) / bestTruck.Speed;
          bestTruck.AddZone(zone, moveTime, 0);

          Zone dump = new Zone(-3,instance.DumpPosition, 0, 0); 
          int toDump = (int)zone.Position.CalculateDistance(dump.Position) / bestTruck.Speed;
          bestTruck.AddZone(dump, toDump, instance.maxDeliveryCapacity);
        }
      }
    }

    foreach (var tv in transportTrucks) {
      currentSolution.Trucks.Add(tv);
      currentSolution.NumVehicles++;
      // currentSolution.TotalTime += tv.CurrentTime;
      currentSolution.TotalLoad += tv.CurrentLoad;
    }

    return currentSolution;
  }
}
