namespace Pract5DAA.LocalSearch;
public class IntraAddLocalSearch : ILocalSearch {
  private Solution _original;
  private Instance _instance;
  public IntraAddLocalSearch(Solution original, Instance instance) {
    _original = original;
    _instance = instance;
  }
  public string GetName => "IntraAddLocalSearch";
  public Solution Solve() {
    Solution bestSolution = _original.Clone();
    int bestTotalTime = bestSolution.TotalTime;
    for (int i = 0; i < bestSolution.Trucks.Count; i++) {
      Truck truck = bestSolution.Trucks[i];
      for (int j = 1; j < truck.Path.Count - 1; j++) {
        Zone currentZone = truck.Path[j];
        if (currentZone.Id < 0) {
          continue;
        }
        for (int k = 1; k < truck.Path.Count - 1; k++) {
          if (k == j) {
            continue;
          }
          Truck truckClone = truck.Clone();
          truckClone.Path.RemoveAt(j);
          truckClone.Path.Insert(k, currentZone);
          Console.WriteLine($"Intentando mover la zona {currentZone.Id} de la posición {j} a la posición {k}");
          Console.WriteLine($"Estado previo de la ruta del camión {truck.Id}: {string.Join(" -> ", truckClone.Path.Select(z => z.Id))}");
          if (!FactibleMovement(truckClone)) {
            continue;
          }
          Console.WriteLine(truckClone);
          Console.WriteLine(truck);
          int newTotalTime = 0;
          foreach (var t in bestSolution.Trucks) {
            newTotalTime += t.CurrentTime;
          }
          if (truckClone.CurrentTime < truck.CurrentTime) {
            bestTotalTime = newTotalTime;
            bestSolution = bestSolution.Clone();
            bestSolution.Trucks[i] = truckClone;
            Console.WriteLine("Mejor solución encontrada:");
            foreach (var t in bestSolution.Trucks) {
              Console.WriteLine($"Camión {t.Id}: {string.Join(" -> ", t.Path.Select(z => z.Id))}");
            }
          }
        }
      }
    }
    return bestSolution;
  }
  public bool FactibleMovement(Truck truck) {
    int currentLoad = 0;
    int currentTime = 0;
    bool firstStationFound = false;
    for (int i = 0; i < truck.Path.Count; i++) {
      Zone currentZone = truck.Path[i];
      if (currentZone.Id == -1 || currentZone.Id == -2) {
        firstStationFound = true;
        break;
      }
      currentLoad += currentZone.Load;
      if (i + 1 < truck.Path.Count) {
        currentTime += currentZone.TimeToNext(truck.Path[i + 1], truck.Speed);
      }
    }
    if (!firstStationFound) {
      return false;
    }
    bool lastStationFound = false;
    for (int i = truck.Path.Count - 1; i >= 0; i--) {
      Zone currentZone = truck.Path[i];
      if (currentZone.Id == -1 || currentZone.Id == -2) {
        lastStationFound = true;
        break;
      }
      currentLoad += currentZone.Load;
      if (i - 1 >= 0) {
        currentTime += currentZone.TimeToNext(truck.Path[i - 1], truck.Speed);
      }
    }
    if (!lastStationFound) {
      return false;
    }
    if (currentLoad > truck.MaximumLoad) {
      return false;
    }
    if (currentTime > truck.MaximumTime) {
      return false;
    }
    Console.WriteLine($"Carga total: {currentLoad}, Tiempo total: {currentTime}");
    return true;
  }
}
