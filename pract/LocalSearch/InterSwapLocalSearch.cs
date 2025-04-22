namespace Pract5DAA.LocalSearch {

  public class InterSwapLocalSearch : ILocalSearch {
    public string GetName => "InterSwapLocalSearch";
    
    private Solution _original;
    private Instance _instance;

    public InterSwapLocalSearch(Solution original, Instance instance) {
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
        Truck truckA = bestSolution.Trucks[i];
        Console.WriteLine($"Procesando camión {i + 1} con ruta: {string.Join(" -> ", truckA.Path.Select(z => z.Id))}");

        // Recorremos cada zona del camión A
        for (int j = 1; j < truckA.Path.Count - 1; j++) {
          Zone zoneA = truckA.Path[j];

          // Si la zona es una estación, la saltamos
          if (zoneA.Id <= 0) continue;

          // Recorremos todos los demás camiones para intentar intercambiar la zona
          for (int k = 0; k < bestSolution.Trucks.Count; k++) {
            if (i == k) continue;  // No podemos intercambiar con el mismo camión

            Truck truckB = bestSolution.Trucks[k];
            Console.WriteLine($"Probando intercambiar zona {zoneA.Id} con el camión {k + 1}");

            // Recorremos cada zona del camión B para probar el intercambio
            for (int l = 1; l < truckB.Path.Count - 1; l++) {
              Zone zoneB = truckB.Path[l];

              // Si la zona es una estación, la saltamos
              if (zoneB.Id <= 0) continue;

              // Crear clon de los camiones para probar el intercambio
              Truck truckAClone = truckA.Clone();
              Truck truckBClone = truckB.Clone();

              // Intercambiamos las zonas entre los camiones
              truckAClone.Path[j] = zoneB; // Colocamos la zona de B en la posición de A
              truckBClone.Path[l] = zoneA; // Colocamos la zona de A en la posición de B

              // Verificamos si el intercambio es factible
              if (!FactibleMovement(truckAClone) || !FactibleMovement(truckBClone)) {
                Console.WriteLine("Intercambio no factible, se omite.");
                continue; // Si no es factible, lo saltamos
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
                bestSolution.Trucks[i] = truckAClone; // Reemplazamos el camión modificado A
                bestSolution.Trucks[k] = truckBClone; // Reemplazamos el camión modificado B
                // bestSolution.TotalTime = newTotalTime; // Actualizamos el tiempo total
              }
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
