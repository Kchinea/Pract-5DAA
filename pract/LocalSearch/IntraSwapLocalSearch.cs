namespace Pract5DAA.LocalSearch {
  public class IntraSwapLocalSearch : ILocalSearch {
    public string GetName => "IntraSwapLocalSearch";
    
    private Solution _original;
    private Instance _instance;

    public IntraSwapLocalSearch(Solution original, Instance instance) {
      _original = original;
      _instance = instance;
    }

    public Solution Solve() {
      // Copiar la solución original para evitar modificarla directamente
      Solution bestSolution = _original.Clone();
  
      // Variable para comparar el tiempo total de la mejor solución encontrada
      int bestTotalTime = bestSolution.TotalTime;
      Console.WriteLine("Tiempo total de la mejor solución inicial: " + bestTotalTime);

      // Recorremos cada camión de la solución
      for (int i = 0; i < bestSolution.Trucks.Count; i++) {
        Truck truck = bestSolution.Trucks[i];
        Console.WriteLine($"Procesando camión {i + 1} con ruta: {string.Join(" -> ", truck.Path.Select(z => z.Id))}");

        // Recorremos cada par de zonas en el camión
        for (int j = 1; j < truck.Path.Count - 1; j++) {
          Zone firstZone = truck.Path[j];

          // Si la zona es una estación, saltamos
          if (firstZone.Id <= 0) {
            continue;
          }

          for (int k = j + 1; k < truck.Path.Count - 1; k++) {
            Zone secondZone = truck.Path[k];

            // Si la zona es una estación, saltamos
            if (secondZone.Id <= 0) {
              continue;
            }

            // Mostrar intercambio que se va a realizar
            Console.WriteLine($"Intentando intercambiar zona {firstZone.Id} en posición {j} con zona {secondZone.Id} en posición {k}");

            // Crear un clon del camión actual para probar el intercambio
            Truck truckClone = truck.Clone();

            // Intercambiamos las zonas en las posiciones j y k
            truckClone.Path[j] = secondZone;
            truckClone.Path[k] = firstZone;

            // Verificar si el intercambio es factible
            if (!FactibleMovement(truckClone)) {
              Console.WriteLine("Movimiento no factible, se omite.");
              continue; // Si no es factible, saltamos
            }

            // Calcular el nuevo tiempo total de la solución con este intercambio
            int newTotalTime = 0;
            foreach (var t in bestSolution.Trucks) {
              newTotalTime += t.CurrentTime;
            }

            // Si encontramos una solución mejor, la actualizamos
            if (newTotalTime < bestTotalTime) {
              Console.WriteLine($"Nueva mejor solución encontrada con tiempo total: {newTotalTime}");
              bestTotalTime = newTotalTime;
              bestSolution = bestSolution.Clone(); // Clonamos la solución para evitar modificar la original
              bestSolution.Trucks[i] = truckClone; // Reemplazamos el camión modificado en la mejor solución
              // bestSolution.TotalTime = newTotalTime; // Actualizamos el tiempo total
            }
          }
        }
      }

      // Devolver la mejor solución encontrada
      Console.WriteLine("Solución final con el mejor tiempo total: " + bestSolution.TotalTime);
      return bestSolution;
    }

    public bool FactibleMovement(Truck truck) {
      int currentLoad = 0;
      int currentTime = 0;

      // Verificar desde la primera zona hasta la primera estación (Id == -1 o -2)
      bool firstStationFound = false;
      for (int i = 0; i < truck.Path.Count; i++) {
        Zone currentZone = truck.Path[i];
        // Verificamos si llegamos a una estación
        if (currentZone.Id == -1 || currentZone.Id == -2) {
          firstStationFound = true;
          break; // Terminamos la validación hasta la primera estación
        }
        // Actualizamos la carga y el tiempo
        currentLoad += currentZone.Load;
        if (i + 1 < truck.Path.Count) {
          currentTime += currentZone.TimeToNext(truck.Path[i + 1], truck.Speed);
        }
      }
      // Si no encontramos la primera estación en la ruta, no es factible
      if (!firstStationFound) {
        return false;
      }

      // Verificar desde la última zona hasta la última estación (Id == -1 o -2)
      bool lastStationFound = false;
      for (int i = truck.Path.Count - 1; i >= 0; i--) {
        Zone currentZone = truck.Path[i];
        // Verificamos si llegamos a una estación
        if (currentZone.Id == -1 || currentZone.Id == -2) {
          lastStationFound = true;
          break; // Terminamos la validación hasta la última estación
        }
        // Actualizamos la carga y el tiempo
        currentLoad += currentZone.Load;
        if (i - 1 >= 0) {
          currentTime += currentZone.TimeToNext(truck.Path[i - 1], truck.Speed);
        }
      }
      // Si no encontramos la última estación en la ruta, no es factible
      if (!lastStationFound) {
        return false;
      }

      // Comprobar la carga total
      if (currentLoad > truck.MaximumLoad) {
        return false; // La carga excede el límite
      }

      // Comprobar el tiempo total
      if (currentTime > truck.MaximumTime) {
        return false; // El tiempo excede el límite
      }

      return true; // Si hemos pasado todas las comprobaciones, el movimiento es factible
    }
  }
}
